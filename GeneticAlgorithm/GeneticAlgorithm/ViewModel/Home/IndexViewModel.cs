using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneticAlgorithm.ViewModel.Home
{
    public class IndexViewModel
    {
        [Required]
        [Display(Name = "Probabilidad de Mutación ")]
        public Int32 Probabilidad { get; set; }

        [Required]
        [Display(Name = "Tamaño Generacional ")]
        public Int32 TamanoGeneracion { get; set; }

        [Required]
        [Display(Name = "Total Generaciones ")]
        public Int32 TotalGeneraciones { get; set; }

        [Required]
        [Display(Name = "Bits en X ")]
        public Int32 BitsX { get; set; }

        [Required]
        [Display(Name = "Bits en Y ")]
        public Int32 BitsY { get; set; }

        //[Required]
        [Display(Name = "Formula para Hallar Valor Genetico")]
        [MaxLength(4)]
        public string Formula { get; set; }

        [Display(Name = "Mejor Valor Genetico")]
        public int MejorGenetico { get; set; }

        [Display(Name = "Generacion del Mejor Valor Genetico")]
        public int GeneracionMejorGenetico { get; set; }

        public List<List<List<int>>> LstGeneracion { get; set; }

        public List<Tuple<List<List<int>>,int>> LstGeneracionMutacion { get; set; }

        public void CargarDatos()
        {
            LstGeneracionMutacion = new List<Tuple<List<List<int>>, int>>();
            LstGeneracion = new List<List<List<int>>>();

            Random r = new Random();
            
            List<List<int>> lstnext = new List<List<int>>();
            List<List<int>> lstback = new List<List<int>>();

            lstnext = lstback = GenerarGeneracion();

            for (int i = 1; i < TotalGeneraciones; i++)
            {
                
               int probTemp = r.Next(0,100);
                if (Probabilidad>= probTemp)
                {
                    lstnext = Mutar(lstback,GenerarMutacion(i + 1));
                    LstGeneracion.Add(lstback);
                    i++;
                    LstGeneracion.Add(lstnext);
                    lstback = lstnext;
                }
                else
                {
                   lstnext = Emparejar( lstback );
                    LstGeneracion.Add(lstnext);
                    lstback = lstnext;
                }

                              

            }


        }

        public List<List<int>> GenerarGeneracion()
        {
            

            List<List<int>> lstlsttemporal = new List<List<int>>();

            Random r = new Random();

            for (int tamano=0;tamano<TamanoGeneracion ;tamano++)
            {
                List<int> lsttemporal = new List<int>();

                for (int i = 0; i < BitsX; i++)
                {
             
                    lsttemporal.Add(r.Next(0, 2));
                }
                for (int i = 0; i < BitsX; i++)
                {
                    lsttemporal.Add(r.Next(0, 2));
                }
                lstlsttemporal.Add(lsttemporal);

                
            }


            LstGeneracion.Add(lstlsttemporal);

            return lstlsttemporal;
        }

        public int HallarValorGenetico(List<int> lstgen)
        {
            int acumuladorx=0;
            int acumuladory = 0;

            for (int i=0; i<BitsX;i++)
            {
                if (lstgen[i]==1)
                {
                    double val = BitsX - 1 - i;
                    val = Math.Pow(2, val);

                    acumuladorx += (int)val;
                }
            }

            for (int i = BitsX; i < BitsY+BitsX; i++)
            {
                if (lstgen[i] == 1)
                {
                    double val = (BitsX + BitsY) - i - 1;
                    val = Math.Pow(2, val);
                    acumuladory += (int)val;
                }
            }

            int respuesta = acumuladorx+ acumuladory*2;

            return respuesta;

        }

        public bool ParejaUsada(List<Tuple<int, int>> tempPareja,int intentoPareja,int buscadorpareja)
        {
            for(int i=0; i<tempPareja.Count;i++)
            {
                if(intentoPareja == tempPareja[i].Item1 || intentoPareja ==tempPareja[i].Item2 )
                {
                    return true;
                }

            }

            return false;
        }
        public Tuple<int, int> ProbarParejaRandom(List<Tuple<int, int>> tempPareja,int buscadorpareja,int maximopareja)
        {

            Random r = new Random();
            bool booltemp = true;
            int intentopareja =r.Next(0,maximopareja);

            while (booltemp)
            {
                intentopareja = r.Next(0,maximopareja);
                if (intentopareja!=buscadorpareja)
                { booltemp = ParejaUsada(tempPareja, intentopareja,buscadorpareja); }
                

            }
            

            return new Tuple<int, int>(buscadorpareja,intentopareja);
        }

        public List<int > fucionarlistas(List<int> lstdereacha,List<int> lstizquierda)
        {
            List<int> temp = new List<int>();

            for(int i=0; i<lstdereacha.Count;i++)
            {
                int valortemp=0;
                if(lstdereacha[i]==1 && lstizquierda[i]==0 || lstdereacha[i] == 0 && lstizquierda[i] == 1)
                {
                    valortemp = 1;
                }

                temp.Add(valortemp);
            }

            return temp;
        }
        public List<List<int>> Emparejar(List<List<int>> lstgene)
        {
            List<List<int>> temp = new List<List<int>>();

            //valor indice
            

            int indicemayor = 0;
            int indicemenor = 0;
            int valormayor = 0 ;
            int valormenor = 999999999;
            temp.Add(lstgene[0]);

            for (int i = 1 ; i < lstgene.Count ; i++) 
            {
                temp.Add(lstgene[i]);
                int valortemporal = HallarValorGenetico(lstgene[i]);
                
                if (valortemporal > valormayor)
                {
                    valormayor = valortemporal;
                    indicemayor = i ;
                }
                if (valortemporal < valormenor)
                {
                    valormenor = valortemporal;
                    indicemenor = i;
                }
            }

            temp[indicemenor] = lstgene[indicemayor];

            ///aparear
            ///
            List<List<int>> temp2 = new List<List<int>>();
            temp2 = temp;
            Random r = new Random();

            List<Tuple<int, int>> temParejas = new List<Tuple<int, int>>();
            temParejas.Add(new Tuple<int, int>(0,r.Next(1,lstgene.Count)));

            int maximocualquiercaso = lstgene.Count / 2;

            for (int i=1; i< lstgene.Count;i++)
            {
                if (!ParejaUsada(temParejas,i,i))
                {
                    temParejas.Add(ProbarParejaRandom(temParejas, i, lstgene.Count));
                }


            }

            
                for (int k=0; k<temParejas.Count;k++)
                {
                    temp2[temParejas[k].Item1] = fucionarlistas(temp[temParejas[k].Item1], temp[temParejas[k].Item2]);
                    temp2[temParejas[k].Item2] = fucionarlistas(temp[temParejas[k].Item1], temp[temParejas[k].Item2]);
            }

            


            return temp2;
        }
        public List<List<int>> Mutar(List<List<int>> lstgene, List<List<int>> lstmutacion )
        {
            List<List<int>> temp = new List<List<int>> ();

            for(int i=0; i<lstmutacion.Count;i++)
            {
                List<int> temp2 = new List<int>();
                for (int j=0; j<lstmutacion[i].Count;j++)
                {
                
                    
                    if(lstgene[i][j]==1 && lstmutacion[i][j]==0 || lstgene[i][j] == 0 && lstmutacion[i][j] == 1)
                    {
                        temp2.Add( 1);
                    }
                    else
                    {
                        temp2.Add(0);
                    }
                    
                    
                }
                temp.Add(temp2);


            }

            return temp;


        }
        public List<List<int>> GenerarMutacion(int indexGeneracion)
        {
            List<List<int>> lstlsttemporal = new List<List<int>>();

            Random r = new Random();

            for (int tamano = 0; tamano < TamanoGeneracion; tamano++)
            {
                List<int> lsttemporal = new List<int>();

                for (int i = 0; i < BitsX; i++)
                {

                    lsttemporal.Add(r.Next(0, 2));
                }
                for (int i = 0; i < BitsX; i++)
                {
                    lsttemporal.Add(r.Next(0, 2));
                }
                lstlsttemporal.Add(lsttemporal);


            }


            LstGeneracionMutacion.Add(new Tuple<List<List<int>>, int>( lstlsttemporal,indexGeneracion-1));
            return lstlsttemporal;
        }

    }
}