﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week3.DB.Entities.DataContext;
using Week3.Model;
using Week3.Model.User;

namespace Week3.Service.User
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;

        public UserService(IMapper _mapper)
        {
            mapper = _mapper;
        }
        // Silme işlemi. Verilen tamamen silinmiyor yalnızca IsActive ve IsDelete kısımları değişiyor.
        public General<UserViewModel> DeleteUser(int id, UserViewModel user)
        {
            var result = new General<UserViewModel>();

            using (var context = new GrootContext())
            {
                var userActivity = context.User.SingleOrDefault(i => i.Id == id);

                if (userActivity is not null)
                {
                    userActivity.IsActive = false;
                    userActivity.IsDeleted = true;

                    context.SaveChanges();

                    result.Entity = mapper.Map<UserViewModel>(userActivity);
                    result.IsSuccess = true;
                    result.Message = "Silme işlemi başarılı!";
                }
                else
                {
                    result.ExceptionMessage = "Aranan kullanıcı bulunamadı. Bilgileri kontrol ediniz.";
                }
            }

            return result;
        }

        // Delete işlemi tüm verileri sildiği için yalnızca deneme amaçlı yazılmıştır!!
        /*
        public General<UserViewModel> DeleteUser(int id)
        {
            var result = new General<UserViewModel>();

            using (var context = new GrootContext())
            {
                var user = context.User.SingleOrDefault(i => i.Id == id);

                if (user is not null)
                {
                    context.User.Remove(user);
                    context.SaveChanges();

                    result.Entity = mapper.Map<UserViewModel>(user);
                    result.IsSuccess = true;
                }
                else
                {
                    result.ExceptionMessage = "Kullanıcı bulunamadı. Bilgileri kontrol ediniz";
                    result.IsSuccess = false;
                }
            }

            return result;
        }

        */

        // Tüm kullanıcıları getiren Request(Get)
        public General<UserViewModel> GetUsers()
        {
            var result = new General<UserViewModel>();

            using (var context = new GrootContext())
            {
                var data = context.User
                    .Where(x => x.IsActive && !x.IsDeleted)
                    .OrderBy(x => x.Id);

                if (data.Any())
                {
                    result.List = mapper.Map<List<UserViewModel>>(data);
                    result.IsSuccess = true;
                    result.Message = "İşlem başarılı!";
                }
                else
                {
                    result.ExceptionMessage = "Sistemde hiçbir kullanıcı yok";
                }
            }

            return result;
        }

        // Yeni bir kullanıcı eklemek için kullanılır(Insert)
        public General<UserViewModel> InsertUser(UserViewModel user)
        {
            var result = new General<UserViewModel>();
            var InsUser = mapper.Map<Week3.DB.Entities.User>(user);

            using (var context = new GrootContext())
            {
                InsUser.Idate = DateTime.Now;
                InsUser.IsActive = true;
                context.User.Add(InsUser);
                context.SaveChanges();

                result.Entity = mapper.Map<UserViewModel>(InsUser);
                result.IsSuccess = true;
                result.Message = "İşlem başarılı !";
            }

            return result;
        }

        // Login işleminde kullanıcı adı ve parola kıyaslaması yapan Request.(Login)
        public General<UserLoginViewModel> Login(UserLoginViewModel user)
        {
            var result = new General<UserLoginViewModel>();
            var logUser = mapper.Map<Week3.DB.Entities.User>(user);

            using (var context = new GrootContext())
            {
                var permission = context.User.Any(x => x.UserName == user.UserName &&
                                         x.IsActive &&
                                         !x.IsDeleted &&
                                         x.Password == user.Password);

                var data = context.User.FirstOrDefault(x => !x.IsDeleted &&
                                                x.IsActive &&
                                                x.UserName == user.UserName &&
                                                x.Password == user.Password);
                if (permission)
                {
                    result.Entity = mapper.Map<UserLoginViewModel>(data);
                    result.IsSuccess = true;
                    result.Message = "İşlem Başarılı !";
                }

                else
                {
                    result.ExceptionMessage = "Kullanıcı bulunamadı. Bilgileri kontrol edin !";
                }
                
            }

            return result;
        }

        // Eğer kullanıcı databasede kayıtlı ise bilgilerini güncellemek için kullanılan Request.(Update)
        public General<UserViewModel> UpdateUser(int id, UserViewModel user)
        {
            var result = new General<UserViewModel>();

            using (var context = new GrootContext())
            {
                var updatedUser = context.User.SingleOrDefault(i => i.Id == id);
                

                if (updatedUser is not null)
                {
                    updatedUser.Name = user.Name;
                    updatedUser.UserName = user.UserName;
                    updatedUser.Email = user.Email;
                    updatedUser.Password = user.Password;

                    context.SaveChanges();

                    result.Entity = mapper.Map<UserViewModel>(updatedUser);
                    result.IsSuccess = true;
                    result.Message = "Güncelleme işlemi başarılı!";
                }
                else
                {
                    result.ExceptionMessage = "Aranan kullanıcı bulunamadı. Bilgileri kontrol ediniz.";
                }
            }

            return result;
        }

       
    }
}
