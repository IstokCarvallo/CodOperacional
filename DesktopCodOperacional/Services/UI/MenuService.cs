using DesktopCodOperacional.Models.Menu;
using System.Collections.ObjectModel;

namespace DesktopCodOperacional.Services.UI
{
    public class MenuService
    {
        public ObservableCollection<MenuItemModel> BuildMenu(string role)
        {
            return role switch
            {
                "Admin" => BuildAdminMenu(),
                _ => BuildUserMenu()
            };
        }

        private ObservableCollection<MenuItemModel> BuildUserMenu()
        {
            return new()
            {
                new()
                {
                    Title = "Dashboard",
                    Icon = "\uE9D2",
                    ViewName = "DashboardView"
                },

                new()
                {
                    Title = "Cuarteles",
                    Icon = "\uE707",
                    ViewName = "CuartelesView"
                },

                new()
                {
                    Title = "Plantas",
                    Icon = "\uE7C3",
                    ViewName = "PlantasView"
                },

                new()
                {
                    Title = "Cambiar Password",
                    Icon = "\uE72E",
                    ViewName = "CambiarPasswordView"
                },

                new()
                {
                    Title = "Logout",
                    Icon = "\uE8AC",
                    ViewName = "Logout"
                }
            };
        }

        private ObservableCollection<MenuItemModel> BuildAdminMenu()
        {
            return new()
            {
                new()
                {
                    Title = "Dashboard",
                    Icon = "\uE9D2",
                    ViewName = "DashboardView"
                },

                new()
                {
                    Title = "Cuarteles",
                    Icon = "\uE707",
                    ViewName = "CuartelesView"
                },

                new()
                {
                    Title = "Plantas",
                    Icon = "\uE7C3",
                    ViewName = "PlantasView"
                },

                new()
                {
                    Title = "Usuarios",
                    Icon = "\uE716",
                    Children =
                    {
                        new()
                        {
                            Title = "Registrar Usuarios",
                            Icon = "\uE710",
                            ViewName = "RegistrarUsuariosView"
                        },

                        new()
                        {
                            Title = "Cambiar Password",
                            Icon = "\uE72E",
                            ViewName = "CambiarPasswordView"
                        }
                    }
                },

                new()
                {
                    Title = "Auditoría",
                    Icon = "\uE7BA",
                    Children =
                    {
                        new()
                        {
                            Title = "Auditoría Completa",
                            ViewName = "AuditoriaCompletaView"
                        },

                        new()
                        {
                            Title = "Auditoría por ID",
                            ViewName = "AuditoriaPorIdView"
                        }
                    }
                },

                new()
                {
                    Title = "Sesiones",
                    Icon = "\uE775",
                    Children =
                    {
                        new()
                        {
                            Title = "Logout Sesiones",
                            ViewName = "LogoutSesionesView"
                        },

                        new()
                        {
                            Title = "Logout Todas",
                            ViewName = "LogoutTodasView"
                        }
                    }
                },

                new()
                {
                    Title = "Logout",
                    Icon = "\uE8AC",
                    ViewName = "Logout"
                }
            };
        }
    }
}
