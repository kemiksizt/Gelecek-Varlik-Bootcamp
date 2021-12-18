﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week3.Model;
using Week3.Model.Product;

namespace Week3.Service.Product
{
    public interface IProductService
    {

        //ProductService içerisinde kullanılacak işlemleri bu interface içerisinde tanımladım.

        public General<ProductViewModel> GetProducts();
        public General<ProductViewModel> InsertProduct(ProductViewModel newProduct);
        public General<ProductViewModel> UpdateProduct(int id, ProductViewModel product);
        //public General<ProductViewModel> DeleteProduct(int id);
        public General<ProductViewModel> GetProductListById(int id, ProductViewModel product);
        public General<ProductViewModel> DeleteProduct(int id, ProductViewModel product);
    }
}
