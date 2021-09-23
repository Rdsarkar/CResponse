using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CResponse.Models;
using CResponse.DTOs;

namespace CResponse
{
    public class SelfClass 
    {
        public decimal? Id { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class Demo1Controller : ControllerBase
    {
        private readonly ModelContext _context;

        public Demo1Controller(ModelContext context)
        {
            _context = context;
        }

        // GET: api/Demo1
        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetDemo1s()
        {
            List<Demo1> Demo1s = await _context.Demo1s
                                                .OrderBy(i=>i.Id)
                                                .ToListAsync();

            if (Demo1s.Count <= 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "NO data Found",
                    Success = false,
                    Payload = null
                });
                
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Get All the data",
                Success = true,
                Payload = Demo1s
            });

        }

        // GET: api/Demo1/5
        [HttpPost("GetDataById")]
        public async Task<ActionResult<ResponseDto>> GetDemo1([FromBody] SelfClass input)
        {
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto 
                {
                    Message = "you have to input ID field",
                    Success = false,
                    Payload = null
                });
            }

            
            var demo1 = await _context.Demo1s.Where(i=>i.Id == input.Id).FirstOrDefaultAsync();
            if (demo1.Id == 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto 
                {
                    Message = "Data Not found in database",
                    Success = false,
                    Payload = null
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseDto 
            {
                Message = "Data Found",
                Success = true,
                Payload = demo1
            });
        }

        // PUT: api/Demo1/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("UpdateData")]
        public async Task<ActionResult<ResponseDto>> PutDemo1([FromBody] Demo1 input)
        {
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto 
                {
                    Message = "You have to input ID",
                    Success = false,
                    Payload = null
                });
            }

            if (input.Name == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "You have to input Name",
                    Success = false,
                    Payload = null
                });
            }

            //old
            Demo1 demo1 = await _context.Demo1s.Where(i => i.Id == input.Id).FirstOrDefaultAsync();
            if (demo1 == null) 
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto
                {
                    Message = "Cant find data in the database",
                    Success = false,
                    Payload = null
                });
            }
            //new
            demo1.Name = input.Name;
            demo1.Email = input.Email;
            //save
            _context.Demo1s.Update(demo1);
            bool isSaved = await _context.SaveChangesAsync() > 0;
            if (isSaved == false) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto 
                {
                    Message = "Data isn't update",
                    Success = false,
                    Payload = null
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseDto 
            {
                Message = "Data Updated",
                Success = true,
                Payload = null
            });
        }

        // POST: api/Demo1
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("InsertingData")]
        public async Task<ActionResult<ResponseDto>> PostDemo1(Demo1 input)
        {
            
            if (input.Id == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "You have to input ID",
                    Success = false,
                    Payload = null
                });
            }

            if (input.Name == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto
                {
                    Message = "You have to input Name",
                    Success = false,
                    Payload = null
                });
            }
            //old 
            Demo1 demo1 = await _context.Demo1s.Where(i => i.Id == input.Id).FirstOrDefaultAsync();
            if (demo1 != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseDto
                {
                    Message = "Same Data Already in the Database",
                    Success = false,
                    Payload = null
                });
            }

            //new

            demo1.Name = input.Name;
            demo1.Id = input.Id;
            demo1.Email = input.Email;
            _context.Demo1s.Add(input);
            bool isSaved = await _context.SaveChangesAsync() > 0;
            if (isSaved == false)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto
                {
                    Message = "Data isn't saved",
                    Success = false,
                    Payload = null
                });
            }
            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "Data Saved",
                Success = true,
                Payload = null
            });
            
        }

        // DELETE: api/Demo1/5
        [HttpPost("ForDeleting")]
        public async Task<ActionResult<ResponseDto>> DeleteDemo1([FromBody] SelfClass input)
        {
            if (input.Id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseDto 
                {
                    Message = "Please Type ID for Deleting",
                    Success = false,
                    Payload = null
                });
            }

            var demo1 = await _context.Demo1s.Where(i=>i.Id == input.Id).FirstOrDefaultAsync();

            
            if (demo1 == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new ResponseDto 
                {
                    Message = "No data Found",
                    Success = false,
                    Payload = null
                });
            }

            _context.Demo1s.Remove(demo1);
            bool isSaved = await _context.SaveChangesAsync() > 0;
            if (isSaved == false) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto 
                {
                    Message = "delete cancel",
                    Success = false,
                    Payload = null
                });
            }

            return StatusCode(StatusCodes.Status200OK, new ResponseDto
            {
                Message = "delete Done",
                Success = true,
                Payload = null
            });
        }

        private bool Demo1Exists(decimal? id)
        {
            return _context.Demo1s.Any(e => e.Id == id);
        }
    }
}
