using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnSale.web.Data;
using OnSaleCommon.Entities;

namespace OnSale.web.Controllers
{
    public class CountriesController : Controller
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context) //inyecta a la base de datos 
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            return View(await _context.countries
                .Include(c=>c.Departments)//incluye los departamentos 
                .ToListAsync());//va ala base deme todos los paises y devuelvamelo a una lista 
        }

      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.countries
                .Include(c=>c.Departments)//equivalente hacer un inner join para que icluya los departamentos
                .ThenInclude(d => d.Cities)//equivalente hacer un inner join para que icluya despus  las ciudades
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

       
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Country country)//devolvemos un pais 
        {
            if (ModelState.IsValid)//si es el modelo es valdo 
            {
                try
                {
                    _context.Add(country);//adicionamos a la base de datos el pais 
                    await _context.SaveChangesAsync();//guardamos los cambios 
                    return RedirectToAction(nameof(Index));//y
                }
                //vamos a ver si se producen excepciones depues que se capture el pais o una actualizacion de la excepcion
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))//si contiene la palabra duplicada 
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");//mensaje de error 
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                //si se revento por cualquier otro motivo 
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }

            }
            return View(country);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //verificamos que el pais exista 
            var country = await _context.countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(country);
        }



        //se modifica el delete que no sea una confirmacion
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            _context.countries.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //adicionar un nuevo departamento auna ciudad 
        public async Task<IActionResult> AddDepartment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Country country = await _context.countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            Department model = new Department { IdCountry = country.Id };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                Country country = await _context.countries
                    .Include(c => c.Departments)
                    .FirstOrDefaultAsync(c => c.Id == department.IdCountry);
                if (country == null)
                {
                    return NotFound();
                }

                try
                {
                    department.Id = 0;
                    country.Departments.Add(department);
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                    return RedirectToAction($"{nameof(Details)}", new { country.Id });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "There are a record with the same name.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            return View(department);
        }

    }
}
