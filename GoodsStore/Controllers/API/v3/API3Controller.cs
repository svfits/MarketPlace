﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoodsStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoodsStore.Controllers.API3
{
    /// <summary>
    /// Третий API контроллер
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v3")]
    public class API3Controller : ControllerBase
    {
        private readonly DataContextApp _context;

        public API3Controller(DataContextApp context)
        {
            _context = context;
        }

        /// <summary>
        /// Получить все значения value и value2
        /// </summary>
        /// <returns>Массив всех данных</returns>
        [HttpGet]
        public async Task<List<APIData>> Get()
        {
            return await _context.APIDatas.ToListAsync();
        }

        /// <summary>
        /// Получить одно значение value по параметру id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Одно значение из API</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var apiData = _context.APIDatas.FirstOrDefault(q => q.Id == id);

            if (apiData == null)
            {
                return NotFound();
            }

            return Ok(await _context.APIDatas.FirstAsync(q => q.Id == id));
        }

        /// <summary>
        /// Добавить значение value
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _context.APIDatas.Add(new APIData()
            {
                Value = value,
                Value2 = value,
            });

            _context.SaveChangesAsync();
        }

        /// <summary>
        /// Чего то обновить
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] APIData data)
        {
            var api = await _context.APIDatas.FirstOrDefaultAsync(m => m.Id == id);
            if (api == null)
            {
                return NotFound();
            }

            api.Value = data.Value;
            api.Value2 = data.Value2;

            _context.APIDatas.Update(api);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Удалить элемент id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var api = await _context.APIDatas.FirstOrDefaultAsync(m => m.Id == id);
            if (api == null)
            {
                return NotFound();
            }

            _context.APIDatas.Remove(api);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
