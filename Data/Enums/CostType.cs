using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Enums
{
    // Типы затрат (Cost) для распределения по проекту
    public enum CostType
    {
        [Display(Name = "Фиксированная оплата")]
        FixedPayment,

        [Display(Name = "Процент прибыли")]
        ProfitPercentage
    }
}
