using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using SquareCalculator.BLL.Abstract;
using System;
using System.Threading;

namespace SquareCalculator.BLL.Services
{
    public class SquareCalculateService : ICalculateService
    {
        private IConfiguration _config;
        private IMemoryCache _cache;

        public SquareCalculateService(IConfiguration config, IMemoryCache cache)
        {
            _config = config;
            _cache = cache;
        }

        public int Calculate(int[] values)
        {
            Validation(values);

            var result = 0;
            foreach (var item in values)
                result += GetSquare(item);
            return result;
        }

        private void Validation(int[] values)
        {
            int maxValuesCount = _config.GetValue<int>("SquareCalculating:MaxValuesCount");
            int maxAllowed = _config.GetValue<int>("SquareCalculating:MaxAllowedNumber");
            int minAllowed = _config.GetValue<int>("SquareCalculating:MinAllowedNumber");

            if (values.Length > maxValuesCount) throw new ArgumentException($"Превышено максимально допустимое количество чисел ({maxValuesCount})");
            foreach (var item in values)
            {
                if (item > maxAllowed) throw new ArgumentException($"Число:{item} больше максимально допустимого ({maxAllowed})");
                if (item < minAllowed) throw new ArgumentException($"Число:{item} меншье минимально допустимого ({minAllowed})");
            }
        }
        private int GetSquare(int number)
        {
            if (_cache.TryGetValue(number, out int result))
                return result;
            result = CalculateSquare(number);
            _cache.Set(number, result);
            return result;

        }
        private int CalculateSquare(int number)
        {
            //Не совсем понял для чего это нужно, мне нужно всё это распаралелить?
            int maxDelay = _config.GetValue<int>("SquareCalculating:MaxDelay");
            int minDelay = _config.GetValue<int>("SquareCalculating:MinDelay");
            Thread.Sleep(new Random().Next(minDelay, maxDelay));
            return (int)Math.Pow(number, 2);
        }

    }
}
