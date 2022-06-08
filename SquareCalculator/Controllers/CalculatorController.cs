using Microsoft.AspNetCore.Mvc;
using SquareCalculator.BLL.Abstract;
using SquareCalculator.Models;
using System;

namespace SquareCalculator.Controllers
{

    //По итогу не реализовал возможность работы с нецелыми числами
    //нет защиты от переполнения и в целом можно ещё много чего доработать
    //но в угоду скорости закончу на этом.
    [ApiController]
    public class CalculatorController : ControllerBase
    {

        private ICalculateService _calculateService;

        public CalculatorController(ICalculateService calculateService)
        {
            _calculateService = calculateService;
        }

        //Post: calculator/square
        [Route("[controller]/[action]")]
        [HttpPost]
        public IActionResult Square([FromBody] SquareCalculatorData data)
        {
            int result;
            try
            {
                result = _calculateService.Calculate(data.Values);
            }
            catch (ArgumentException ex)
            {
                return new JsonResult(new { Status = "error", ErrorMessage = ex.Message });
            }

            return new JsonResult(new { Status = "ok", Result = result});
        }
    }
}
