//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace test_for_sber.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class group
    {
        public group()
        {
            this.products = new HashSet<products>();
        }
    
        public string title_group { get; set; }
        public int id_group { get; set; }
    
        public virtual ICollection<products> products { get; set; }
    }
}
