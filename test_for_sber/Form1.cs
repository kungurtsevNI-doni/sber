using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Caching;

namespace test_for_sber
{
    public partial class Form1 : Form
    {
        private Entities.priceEntities context = new Entities.priceEntities(); //Объект контекст.
        private ObjectCache cache = MemoryCache.Default; //Объект кэша.
        public Form1()
        {   
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var groups = context.group.ToList(); //Лист авторов.
            //В лист объектов заносим пустой, дабы изначально ни одна группа не была выбрана.
            List<Entities.group> list = new List<Entities.group>
            {
                new Entities.group {title_group=""},
            };
            //Все объекты групп заносим в список.
            foreach (var group in groups) {
                list.Add(new Entities.group { id_group = group.id_group, title_group = group.title_group });
            }
            comboBox1.DataSource = list; //Устанавливаем источник данным для выпадающего списка.
            comboBox1.DisplayMember = "title_group"; //Устанавливаем то, что будет видеть пользователь.
            comboBox1.ValueMember = "id_group"; //Устанавливаем значение, которое потребуется для определения списка продуктов.
        }

        private void comboBox1_SelectionChangeCommitted_1(object sender, EventArgs e)
        {
            var group = comboBox1.SelectedValue; //Получаем значение (id_group) элемента списка, которое выбрал пользователь.
            CacheItem ch = cache.GetCacheItem(group.ToString()); //Получаем объект из кэша по id.
            //Если объект из кэша пустой, получаем продукты и заносим их в кэш.
            if (ch == null)
            {
                //Продукты с соответствующим id группы.
                var products = context.products.Select(c => new
                                                {
                                                    id_product = c.id_product,
                                                    title_product = c.title_product,
                                                    id_group = c.id_group
                                                })
                                                .Where(c => c.id_group == (int)group)
                                                .ToList(); 
                // Cоздаем новый объект CacheItemPolicy, указывающий, что срок действия записи кэша истекает через 1000 секунд.
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(1000.0);
                //Присваеваем объекту item кэша значение = продуктам, ключ = id_group
                ch = new CacheItem(group.ToString(), products);
                //Заносим объект ch в кэш.
                cache.Set(ch, policy);
                //Выводим продукты из кэша.
                dataGridView1.DataSource = cache.GetCacheItem(group.ToString()).Value;
            }//Если объект по данному ключу уже есть в кэше, то его и выводим в таблицу.
            else {
                //Выводим продукты из кэша.
                dataGridView1.DataSource = cache.GetCacheItem(group.ToString()).Value;
            }  
        }
    }
}
