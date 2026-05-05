using APISegura.Common;
using APISegura.Entities;
using APISegura.Repositories.Interfaces;
using APISegura.Services.Interfaces;

namespace APISegura.Services
{
    public class PasswordRecoveryService : IPasswordRecoveryService
    {
        private readonly IUserRepository _userRepo;
        private readonly IPasswordResetRepository _resetRepo;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly IEmailService _emailService;
        private readonly TokenService _tokenService;
        private readonly PasswordService _pwd;

        public PasswordRecoveryService(
            IUserRepository userRepo,
            IPasswordResetRepository resetRepo,
            IRefreshTokenRepository refreshRepo,
            IEmailService emailService, 
            TokenService tokenService,
            PasswordService passwordservices)
        {
            _userRepo = userRepo;
            _resetRepo = resetRepo;
            _refreshRepo = refreshRepo;
            _emailService = emailService;
            _tokenService = tokenService;
            _pwd = passwordservices;
        }

        public async Task<Result<bool>> ForgotPassword(string email)
        {
            var user = await _userRepo.GetByEmail(email);

            if (user == null)
                return Result<bool>.Ok(true);

            // invalidar tokens anteriores
            await _resetRepo.InvalidateAllByUser(user.Id);

            var token = _tokenService.GenerateToken();
            var tokenHash = _tokenService.HashToken(token);

            await _resetRepo.Create(new PasswordResetToken
            {
                UserId = user.Id,
                TokenHash = tokenHash,
                Expiration = DateTime.UtcNow.AddMinutes(15)
            });

            await _emailService.SendPasswordReset(user.Email, token);

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> ResetPassword(string token, string newPassword)
        {
            var record = await _resetRepo.GetByToken(token);

            if (record == null || record.IsUsed || record.Expiration < DateTime.UtcNow)
                return Result<bool>.Fail("Token inválido");

            var user = await _userRepo.GetById(record.UserId);

            // hash PBKDF2
            var (hash, salt, iterations) = _pwd.HashPassword(newPassword);

            user.PasswordHash = hash;
            user.PasswordSalt = salt;
            user.Iterations = iterations;

            await _userRepo.UpdatePassword(user);

            // invalidar token usado
            record.IsUsed = true;
            await _resetRepo.Update(record);

            // revocar sesiones activas
            await _refreshRepo.RevokeAllByUser(user.Id);

            return Result<bool>.Ok(true);
        }
    }
}
