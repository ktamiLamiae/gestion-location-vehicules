using GestionLocationVehicule.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionLocationVehicule.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly GestionLocationContext _context;

        // Injection du contexte via le constructeur
        public AuthController(GestionLocationContext context)
        {
            _context = context;
        }


        // GET: /Auth/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string MotDePasse)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(MotDePasse))
            {
                ViewBag.ErrorMessage = "Veuillez remplir tous les champs.";
                return View();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == Email && c.MotDePasse == MotDePasse);

            if (client == null)
            {
                ViewBag.ErrorMessage = "Email ou mot de passe incorrect.";
                return View();
            }

            // 🔐 SAUVEGARDER DANS LA SESSION
            HttpContext.Session.SetInt32("UserId", client.IdClient);
            HttpContext.Session.SetString("UserEmail", client.Email);
            HttpContext.Session.SetString("UserNom", client.Nom);
            HttpContext.Session.SetString("UserType", client.TypeUtilisateur);

            if (client.TypeUtilisateur == "Admin")
            {
                return RedirectToAction("Index", "Clients", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "User", new { area = "Client" });
            }
        }



        // Créer un utilisateur admin (pour admin)
        public IActionResult Create()
        {
            ViewBag.IsAdmin = true;
            return View();
        }
        // Créer un client (client non admin)
        public IActionResult CreateClient()
        {
            ViewBag.IsAdmin = false;
            return View("~/Areas/Admin/Views/Clients/Create.cshtml");
        }

        [HttpPost] // Il est plus sûr d'utiliser POST pour le logout
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Supprime le cookie d'authentification
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirige vers la page de Login (ou l'accueil)
            return RedirectToAction("Login", "Auth", new { area = "Admin" });
        }


    }
}