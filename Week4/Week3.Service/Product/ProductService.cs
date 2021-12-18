using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week3.DB.Entities.DataContext;
using Week3.Model;
using Week3.Model.Product;

namespace Week3.Service.Product
{
    public class ProductService : IProductService
    {
        private readonly IMapper mapper;

        public ProductService(IMapper _mapper)
        {
            mapper = _mapper;
        }

        // Sistemdeki belirtilen id grubuna ait bütün ürünleri getiren Request(GetById)
        public General<ProductViewModel> GetProductListById(int id, ProductViewModel product)
        {
            var result = new General<ProductViewModel>();

            using (var context = new GrootContext())
            {
                var data = context.User
                    .Where(x => x.IsActive && !x.IsDeleted && x.Iuser == id)
                    .OrderBy(x => x.Id);

                if (data.Any())
                {
                    result.List = mapper.Map<List<ProductViewModel>>(data);
                    result.IsSuccess = true;
                    result.Message = "Listeleme işlemi başarılı!";
                }
                else
                {
                    result.ExceptionMessage = "Sistemde hiçbir ürün yok";
                }
            }

            return result;
        }
        // Sistemdeki ürüne ait tüm özellikleri silen Request. Deneme amaçlı yapılmıştır!!
        /*
        public General<ProductViewModel> DeleteProduct(int id)
        {
           var result = new General<ProductViewModel>();

           using (var context = new GrootContext())
           {
               var product = context.Product.SingleOrDefault(i => i.Id == id);

               if (product is not null)
               {
                   context.Product.Remove(product);
                   context.SaveChanges();

                   result.Entity = mapper.Map<ProductViewModel>(product);
                   result.IsSuccess = true;
               }
               else
               {
                   result.ExceptionMessage = "Ürün bulunamadı. Bilgileri kontrol ediniz";
                   result.IsSuccess = false;
               }
           }

           return result;
        }
        */

        // Sistemdeki bütün ürünleri getiren Request(Get)
        public General<ProductViewModel> GetProducts()
        {
            var products = new General<ProductViewModel>();

            using (var context = new GrootContext())
            {
                var data = context.Product
                    .Where(x => x.IsActive && !x.IsDeleted)
                    .OrderBy(x => x.Id);

                if (data.Any())
                {
                    products.List = mapper.Map<List<ProductViewModel>>(data);
                    products.IsSuccess = true;
                    products.Message = "Listeleme işlemi başarılı!";
                }
                else
                {
                    products.ExceptionMessage = "Sistemde hiçbir ürün yok";
                }
            }

            return products;
        }

        // Sisteme yeni ürün eklemek için kullanılan Request(Insert) 
        public General<ProductViewModel> InsertProduct(ProductViewModel product)
        {
            var data = new General<ProductViewModel>();
            var InsProduct = mapper.Map<Week3.DB.Entities.Product>(product);

            using (var context = new GrootContext())
            {
                var permission = context.User.Any(x => x.Id == InsProduct.IUser && x.IsActive && !x.IsDeleted);

                if (permission)
                {
                    InsProduct.Idate = DateTime.Now;
                    InsProduct.IsActive = true;
                    context.Product.Add(InsProduct);
                    context.SaveChanges();

                    data.Entity = mapper.Map<ProductViewModel>(InsProduct);
                    data.IsSuccess = true;
                    data.Message = "Kayıt ekleme işlemi başarılı";
                }

                else
                {
                    data.ExceptionMessage = "Ürün ekleme yetkiniz yok !";
                }


            }
            return data;
        }


        // Var olan ürünün özelliklerini değiştirmek için kullanılan Request.(Update)
        public General<ProductViewModel> UpdateProduct(int id, ProductViewModel product)
        {
            var data = new General<ProductViewModel>();

            using (var context = new GrootContext())
            {
                var updatedProduct = context.Product.SingleOrDefault(i => i.Id == id);
                var permission = context.Product.Any(x => x.IUser == product.IUser);

                if (permission)
                {
                    if (updatedProduct is not null)
                    {
                        updatedProduct.Name = product.Name;
                        updatedProduct.DisplayName = product.DisplayName;
                        updatedProduct.Description = product.Description;
                        updatedProduct.Price = product.Price;
                        updatedProduct.Stock = product.Stock;

                        context.SaveChanges();

                        data.Entity = mapper.Map<ProductViewModel>(updatedProduct);
                        data.IsSuccess = true;
                        data.Message = "Güncelleme işlemi başarılı!!";
                    }
                    else
                    {
                        data.ExceptionMessage = "Aranan ürün bulunamadı. Bilgileri kontrol ediniz.";
                    }
                }

                else
                {
                    data.ExceptionMessage = "Ürün ekleme yetkiniz yok !";
                }
            }

            return data;
        }

        // Silme işlemi. Verilen tamamen silinmiyor yalnızca IsActive ve IsDelete kısımları değişiyor.
        public General<ProductViewModel> DeleteProduct(int id, ProductViewModel product)
        {
            var result = new General<ProductViewModel>();

            using (var context = new GrootContext())
            {
                var productActivity = context.Product.SingleOrDefault(i => i.Id == id);
                var permission = context.Product.Any(x => x.IUser == product.IUser);

                if (permission)
                {
                    if (productActivity is not null)
                    {
                        productActivity.IsActive = false;
                        productActivity.IsDeleted = true;

                        context.SaveChanges();

                        result.Entity = mapper.Map<ProductViewModel>(productActivity);
                        result.IsSuccess = true;
                        result.Message = "Silme işlemi başarılı!";
                    }
                    else
                    {
                        result.ExceptionMessage = "Aranan ürün bulunamadı. Bilgileri kontrol ediniz.";
                    }
                }
                else
                {
                    result.ExceptionMessage = "Ürün silme yetkiniz yok !";
                }

               
            }

            return result;
        }

        // Sıralama işlemlerinin olduğu bölüm. İsme ve fiyata göre artan ve ya azalan şeklinde seçim yapılabilir.
        public General<ListProductViewModel> SortProduct(string param)
        {
            var result = new General<ListProductViewModel>();

            using (var context = new GrootContext())
            {
                var products = context.Product.Where(x => x.IsActive && !x.IsDeleted && x.Id > 0);

                if (param.Equals("PriceASC"))
                {
                    products = products.OrderBy(x => x.Price);
                }

                else if (param.Equals("PriceDESC"))
                {
                    products = products.OrderByDescending(x => x.Price);
                }

                else if (param.Equals("NameASC"))
                {
                    products = products.OrderBy(x => x.Name);
                }

                else if (param.Equals("NameDESC"))
                {
                    products = products.OrderByDescending(x => x.Name);
                }

                else
                {
                    result.ExceptionMessage = "Yanlış işlem seçtiniz.";

                    return result;
                    
                }
                result.IsSuccess = true;
                result.List = mapper.Map<List<ListProductViewModel>>(products);
                result.Message = "Sıralama işlemi başarılı!";
               

            }

            return result;
        }

        // Filtreleme işlemi. Beklenenen isim özelliğine sahip ürünler getirilir.
        public General<ListProductViewModel> FilterProduct(string param)
        {
            var result = new General<ListProductViewModel>();
            using (var context = new GrootContext())
            {
                var products = context.Product.Where(x => x.IsActive && !x.IsDeleted && x.Id > 0);

                products = products.Where(x => x.Name.StartsWith(param));
                if (products.Any() )
                {
                    result.IsSuccess = true;
                    result.List = mapper.Map<List<ListProductViewModel>>(products);
                    result.Message = "Filtreleme işlemi başarılı";
                }
                else
                {
                    result.ExceptionMessage = "İstenilen ürün bulunamadı. Tekrar deneyin!";
                    return result;
                }
                
            }

            return result;
        }

        // Sayfalama işlemi. Eğer istenilen sayfa sayısı ve sayfadaki ürün sayısı özellikleri toplam ürün sayısına
        // uygun değilse hataya özel hata mesajları döner. X. sayfa Y adet ürün şeklinde çalışır.
        public General<ListProductViewModel> PaginateProduct(int productByPage, int pageNo)
        {
            var result = new General<ListProductViewModel>();

            decimal pageCount = 0;
            decimal productCount = 0;
            


            using (var context = new GrootContext())
            {

                result.ProductCount = context.Product.Count();
                productCount = result.ProductCount;

                pageCount = Math.Ceiling(productCount / productByPage);
                var products = context.Product.OrderBy(i => i.Id).Skip((int)((pageNo - 1) * productByPage)).Take((int)productByPage).ToList();


                if (productByPage <= 0 || pageNo <= 0)
                {
                    result.ExceptionMessage = "Yanlış değer girdiniz!";
                }

                else if(productByPage > productCount)
                {
                    result.ExceptionMessage = "Bu kadar ürün mevcut değil! Sayıyı değiştirin.";
                }

                else if(pageNo > productCount)
                {
                    result.ExceptionMessage = "Bu kadar sayfa mevcut değil! Tekrar deneyin.";
                }

                else
                {
                  
                    result.List = mapper.Map<List<ListProductViewModel>>(products);
                    result.IsSuccess = true;
                    result.Message = "Sayfalama işlemi başarılı.";
                    
                }
                
            }

            result.pageCount = pageCount;

            return result;
        }



    }
}
