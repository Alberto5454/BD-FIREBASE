using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

using Newtonsoft.Json;

using BDFIREBASE.Models;

namespace BDFIREBASE.Controllers
{
    public class MantenedorController : Controller
    {


        IFirebaseClient Cliente;


        public MantenedorController() {

            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "dIYjNiFzQOtqRbuXOGXvEsVRmEIvzFKBi9Vp8M1j",
                BasePath = "https://bdfirebase-24b7f-default-rtdb.firebaseio.com/"

            };

            Cliente = new FirebaseClient(config);

        }

       
        
        
        // GET: Mantenedor
        public ActionResult Inicio()
        {
            Dictionary<string,Contacto> lista = new Dictionary<string,Contacto>();
            FirebaseResponse response = Cliente.Get("contactos");

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
               lista = JsonConvert.DeserializeObject<Dictionary<string, Contacto>>(response.Body);


            List<Contacto> ListaContacto = new List<Contacto>();

            foreach(KeyValuePair<string,Contacto> elemento in lista) {

                ListaContacto.Add(new Contacto() {
                    IdContacto = elemento.Key,
                    Nombre = elemento.Value.Nombre,
                    Correo = elemento.Value.Correo,
                    Telefono = elemento.Value.Telefono,
                });

            }

            return View(ListaContacto);
        }
        public ActionResult crear()
        {
            return View();
        }



        public ActionResult editar(string idcontacto)
        {

            FirebaseResponse response = Cliente.Get("contactos/" + idcontacto);

            Contacto ocontacto = response.ResultAs<Contacto>();
            ocontacto.IdContacto = idcontacto;

            return View(ocontacto);
        }

        public ActionResult eliminar(string idcontacto)
        {
            FirebaseResponse response = Cliente.Delete("contactos/" + idcontacto);
            return RedirectToAction("Inicio", "Mantenedor");
        }



        [HttpPost]
        public ActionResult crear(Contacto oContacto)
        {
            string IdGenerado = Guid.NewGuid().ToString("N");

            SetResponse response = Cliente.Set("contactos/"+ IdGenerado, oContacto);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Inicio", "Mantenedor");
            }
            else {
                return View();
            }

        }

        [HttpPost]
        public ActionResult editar(Contacto oContacto)
        {

            string idcontacto = oContacto.IdContacto;
            oContacto.IdContacto = null;

            FirebaseResponse response = Cliente.Update("contactos/" + idcontacto,oContacto);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Inicio", "Mantenedor");
            }
            else
            {
                return View();
            }
        }





    }
}