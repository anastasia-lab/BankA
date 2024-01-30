using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankA.Services
{
    class Interfaces
    {
        public interface IAccount<T> : ICovariance<T>, IContrvariance<T>
        {
            T Balance { get; }
            bool IsOpen { get; set; }
        }

        public interface ICovariance<out T>
        {
           void GetValue(long amount);
        }

        public interface IContrvariance<in T>
        {
            void SetValue(T value);
        }
    }
}
