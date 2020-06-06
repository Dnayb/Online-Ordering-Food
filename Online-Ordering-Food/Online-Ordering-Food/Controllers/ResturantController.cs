using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Ordering_Food.Models;
using System.IO;
using Online_Ordering_Food.ViewModel;

namespace Online_Ordering_Food.Controllers
{
    public class ResturantController : Controller
    {
        private ResturantEntities db = new ResturantEntities();

        public ActionResult OOFMenu()
        {
            var viewmodel = new ProductCart()
            {
                products = db.Products.Include(p => p.Category).ToList(),
                carts = db.Carts.Include(c => c.Product).ToList()
            };
            //var products = db.Products.Include(p => p.Category);
            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult OOFMenu(string SearchTerm)
        {

            var viewmodel = new ProductCart()
            {
                products = db.Products.Include(p => p.Category).ToList(),
                carts = db.Carts.Include(c => c.Product).ToList()
            };
            if (string.IsNullOrEmpty(SearchTerm))
            {
                viewmodel.products = db.Products.Include(p => p.Category).ToList();
            }
            else
            {
                viewmodel.products = db.Products.Include(p => p.Category).Where(a => a.Category.Category_Name.ToLower().StartsWith(SearchTerm.ToLower())).ToList();
            }
            return View(viewmodel);
        }

        // Menu page
        public ActionResult Menu()
        {

            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        [HttpPost]
        public ActionResult Menu(string SearchTerm)
        {
            List<Product> products;
            if (string.IsNullOrEmpty(SearchTerm))
            {
                products = db.Products.Include(p => p.Category).ToList();
            }
            else
            {
                products = db.Products.Include(p => p.Category).Where(a => a.Category.Category_Name.ToLower().StartsWith(SearchTerm.ToLower())).ToList();
            }
            return View(products);
        }

        public ActionResult MenuDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //public ActionResult OOFMenu()
        //{

        //    var products = db.Products.Include(p => p.Category);
        //    return View(products.ToList());
        //}

        //[HttpPost]
        //public ActionResult OOFMenu(string SearchTerm)
        //{
        //    List<Product> products;
        //    if (string.IsNullOrEmpty(SearchTerm))
        //    {
        //        products = db.Products.Include(p => p.Category).ToList();
        //    }
        //    else
        //    {
        //        products = db.Products.Include(p => p.Category).Where(a => a.Category.Category_Name.ToLower().StartsWith(SearchTerm.ToLower())).ToList();
        //    }
        //    return View(products);
        //}

        public ActionResult OOFMenuDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        //Home Page:
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult OOFHome()
        {
            return View();
        }

        //Regestration Page
        [HttpGet]
        [ActionName("Regestration")]
        public ActionResult Regestration_Get()
        {
            return View();
        }


        [HttpPost]
        [ActionName("Regestration")]
        public ActionResult Regestration_Post(UserInfo user)
        {
            using (ResturantEntities db = new ResturantEntities())
            {
                if (db.UserInfoes.Any(x => x.username == user.username) || db.UserInfoes.Any(y => y.EmailAddress == user.EmailAddress))
                {
                    ViewBag.message = "UserName OR Email Already Exist";
                    return View(user);
                }
                else
                {
                    db.UserInfoes.Add(user);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("LogIn");
        }

        //login page
        [HttpGet]
        [ActionName("LogIn")]
        public ActionResult LogIn_Get()
        {

            return View();
        }

        [HttpPost]
        [ActionName("LogIn")]
        public ActionResult LogIn_Post(UserInfo user)
        {
            ResturantEntities db = new ResturantEntities();

            if ((user.username == "admin") && (user.Password == "admin1234"))
            {
                return RedirectToAction("AdminDashboard");
            }
            else if (db.UserInfoes.Any(x => x.username == user.username) && db.UserInfoes.Any(y => y.Password == user.Password))
            {
                return RedirectToAction("OOFHome");
            }
            else
            {
                ViewBag.message = "username or Password is INCORRECT";
                return View(user);
            }
        }


        // admin pages
        public ActionResult AdminDashboard()
        {
            return View();
        }

        //ProductDetails Page
        public ActionResult ProductDetails()
        {

            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        [HttpPost]
        public ActionResult ProductDetails(string SearchTerm)
        {
            List<Product> products;
            if (string.IsNullOrEmpty(SearchTerm))
            {
                products = db.Products.Include(p => p.Category).ToList();
            }
            else
            {
                products = db.Products.Include(p => p.Category).Where(a => a.Category.Category_Name.ToLower().StartsWith(SearchTerm.ToLower())).ToList();
            }
            return View(products);
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // CreateProduct page
        public ActionResult Create()
        {
            ViewBag.Category_id = new SelectList(db.Categories, "Id", "Category_Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,Image,Description,Category_id")] Product product, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                upload.SaveAs(path);
                product.Image = upload.FileName;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("ProductDetails");
            }

            ViewBag.Category_id = new SelectList(db.Categories, "Id", "Category_Name", product.Category_id);
            return View(product);
        }

        // UpdateProduct Page
        public ActionResult UpdateProduct()
        {

            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        [HttpPost]
        public ActionResult UpdateProduct(string SearchTerm)
        {
            List<Product> products;
            if (string.IsNullOrEmpty(SearchTerm))
            {
                products = db.Products.Include(p => p.Category).ToList();
            }
            else
            {
                products = db.Products.Include(p => p.Category).Where(a => a.Category.Category_Name.ToLower().StartsWith(SearchTerm.ToLower())).ToList();
            }
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Category_id = new SelectList(db.Categories, "Id", "Category_Name", product.Category_id);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,Image,Description,Category_id")] Product product, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                upload.SaveAs(path);
                product.Image = upload.FileName;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProductDetails");
            }
            ViewBag.Category_id = new SelectList(db.Categories, "Id", "Category_Name", product.Category_id);
            return View(product);
        }

        // DeleteProduct Page

        public ActionResult DeleteProduct()
        {

            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        [HttpPost]
        public ActionResult DeleteProduct(string SearchTerm)
        {
            List<Product> products;
            if (string.IsNullOrEmpty(SearchTerm))
            {
                products = db.Products.Include(p => p.Category).ToList();
            }
            else
            {
                products = db.Products.Include(p => p.Category).Where(a => a.Category.Category_Name.ToLower().StartsWith(SearchTerm.ToLower())).ToList();
            }
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            Product product = db.Products.Find(id);
            if (db.Carts.Find(id) != null)
            {
                db.Products.Remove(product);
                db.Carts.Remove(cart);
                db.SaveChanges();
            }
            else
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }
            return RedirectToAction("DeleteProduct");
        }

        //
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult AddToCart(int id)
        {
            var productInCart = new Cart();
            var product = db.Carts.SingleOrDefault(c => c.product_id == id);
            if (product != null)
            {
                return RedirectToAction("OOFMenu");
            }
            else
            {
                productInCart.product_id = id;
                productInCart.added_at = DateTime.Now;
                db.Carts.Add(productInCart);
                db.SaveChanges();
                return RedirectToAction("OOFMenu");
            }
        }
        public ActionResult DeleteCartItem(int id)
        {
            var cartproduct = db.Carts.Single(Cart => Cart.product_id == id);
            db.Carts.Remove(cartproduct);
            db.SaveChanges();
            return RedirectToAction("OOFMenu");
        }
    }
}
