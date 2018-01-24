﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MonsterHunterAPI.Models;
using MonsterHunterAPI.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MonsterHunterAPI.Controllers
{
    [Route("api/[controller]")]
    public class BladeController : Controller
    {
        private readonly HunterDbContext _context;

        public BladeController(HunterDbContext context)
        {
            _context = context;
        }

        // GET: api/blade
        [HttpGet]
        public IEnumerable<Blade> Get() => _context.Blades;

        // GET api/blade/blade/:id
        [HttpGet("{id:int}")]
        public Blade Blade(int id)
        {
            Blade newBlade = _context.Blades.FirstOrDefault(b => b.ID == id);
            newBlade.Materials = new List<string>();
            List<BladeMaterial> newBladeMaterials = _context.BladesMaterials.Where(y => y.Blade.ID == id).ToList();
            foreach(var x in newBladeMaterials)
            {
                List<Material> materials = _context.Materials.Where(m => m.ID == x.MaterialID).ToList();
                
                foreach (var y in materials)
                {
                    newBlade.Materials.Add(y.Name + ":" + x.Quantity);
                }
            }
            return newBlade;
        }

        // GET api/blade/filterBy/:weaponClass/:element/:rarity
        [HttpGet("{weaponClass}/{element?}/{rarity:int?}")]
        public List<Blade> FilterBy(string weaponClass, string element, int? rarity)
        {
            List<Blade> bladesToReturn = new List<Blade>();
            bladesToReturn = _context.Blades.ToList();

            if (!String.IsNullOrEmpty(weaponClass))
            {
                bladesToReturn = bladesToReturn.Where(b => b.WeaponClass == weaponClass).ToList();
            }

            if (!String.IsNullOrEmpty(element))
            {
                bladesToReturn = bladesToReturn.Where(b => b.ElementType == element).ToList();
            }

            if (rarity.HasValue)
            {
                bladesToReturn = bladesToReturn.Where(b => b.Rarity == rarity).ToList();
            }

            return bladesToReturn;
        }

        // POST: api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Blade value)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            BladeMaterial newBMrelation = new BladeMaterial(); ;

            // Parsing materials and quantities as list of strings, to update BladeMaterial table
            foreach(string s in value.Materials)
            {
                string[] values = s.Split(':');
                Material relatedMaterial = _context.Materials.FirstOrDefault(x => x.Name == values[0]);
                newBMrelation.MaterialID = relatedMaterial.ID;
                newBMrelation.Quantity = Int32.Parse(values[1]);
            }

            await _context.Blades.AddAsync(value);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { value.ID }, value);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
