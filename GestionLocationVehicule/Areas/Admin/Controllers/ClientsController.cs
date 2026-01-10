using GestionLocationVehicule.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestionLocationVehicule.Areas_Admin_Controllers
{
    [Area("Admin")]
    public class ClientsController : Controller
    {
        private readonly GestionLocationContext _context;

        public ClientsController(GestionLocationContext context)
        {
            _context = context;
        }
        // GET: Clients/CreateClient
        [HttpGet]
        [AllowAnonymous] // Permet aux utilisateurs non connectés d'accéder
        public IActionResult CreateClient()
        {
            ViewBag.IsAdmin = false; // pour le formulaire
            return View("Create"); // on utilise la même vue Create.cshtml
        }

        // POST: Clients/CreateClient
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateClient([Bind("Nom,Prenom,Adresse,Ville,NumeroTelephone,Email,MotDePasse")] Client client)
        {
            // Sécurité : le client ne peut jamais se créer en admin
            client.TypeUtilisateur = "Client";

            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();

                // Après création, rediriger vers login
                return RedirectToAction("Login", "Auth", new { area = "Admin" });
            }

            ViewBag.IsAdmin = false;
            return View("Create", client);
        }

        // GET: Clients
        //public async Task<IActionResult> Index()
        // {
        //   return View(await _context.Clients.ToListAsync());
        // }
        public async Task<IActionResult> Index(string searchString)
        {
            var clients = from c in _context.Clients
                          select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(c => c.Nom.Contains(searchString)
                                          || c.Prenom.Contains(searchString)
                                          || c.Ville.Contains(searchString));
            }

            return View(await clients.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.IdClient == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewBag.IsAdmin = true;
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nom,Prenom,Adresse,Ville,NumeroTelephone,Email,MotDePasse,TypeUtilisateur")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdClient,Nom,Prenom,Adresse,Ville,NumeroTelephone,Email,MotDePasse,TypeUtilisateur")] Client client)
        {
            if (id != client.IdClient)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.IdClient))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.IdClient == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.IdClient == id);
        }
    }
}
