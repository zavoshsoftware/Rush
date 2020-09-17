﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Models;

namespace Helpers
{
    public class ZarinPalHelper
    {
        private String MerchantId = "28370b76-bf5f-11ea-8f7a-000c295eb8fc";
        private DatabaseContext db = new DatabaseContext();

        public string GetMerchantId()
        {
            return MerchantId;
        }
        public string ZarinPalRedirect(Order order, decimal amount)
        {
            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();

           String CallbackURL = "http://localhost:59339/callback";
           //String CallbackURL = "https://www.rushweb.ir/callback";

            long Amount = Convert.ToInt64(amount);

            List<OrderDetail> orderDetails = db.OrderDetails.Where(current => current.OrderId == order.Id).Include(c=>c.Product).ToList();
            string productDesc = null;
            foreach (OrderDetail orderDetail in orderDetails)
            {

              //  productDesc = productDesc + orderDetail.Product. + ",";
            }

            String description = "خرید محصول ";

            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantId, Amount, CallbackURL, description);

            zarinpal.EnableSandboxMode();
            try
            {
                var res = zarinpal.InvokePaymentRequest(pr);
                if (res.Status == 100)
                {
                    InsertToAuthority(order.Id, res.Authority, amount);

                    return res.PaymentURL;
                }
                else
                    return "false";


            }
            catch (Exception e)
            {
                return "zarrin";
            }
        }
        

        public void InsertToAuthority(Guid orderId, string authority, decimal amount)
        {
            ZarinpallAuthority zarinpallAuthority = new ZarinpallAuthority()
            {
                OrderId = orderId,
                Authority = authority,
                Amount = amount,
                CreationDate = DateTime.Now,
                IsDeleted = false,
                IsActive = true
            };

            db.ZarinpallAuthorities.Add(zarinpallAuthority);
            db.SaveChanges();
        }

        public long GetAmountByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
                db.ZarinpallAuthorities.FirstOrDefault(current => current.Authority == authority);

            if (zarinpallAuthority != null)
                return Convert.ToInt64(zarinpallAuthority.Amount);

            return 0;
        }

        public Order GetOrderByAuthority(string authority)
        {
            ZarinpallAuthority zarinpallAuthority =
                db.ZarinpallAuthorities.FirstOrDefault(current => current.Authority == authority);

            if (zarinpallAuthority != null)
                return zarinpallAuthority.Order;

            else
                return null;
        }
    }
}