using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts;

namespace XCharts
{
    public class Test : MonoBehaviour
    {
        public PieChart chart;
        public RadarChart radarChart;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Series seri = chart.series;
                Serie serie = seri.GetSerie(0);
                serie.name = "成分";
                seri.ClearData();
                seri.AddData("成分", 60, "有机质");
                seri.AddData("成分", 18, "沙子");
                seri.AddData("成分", 22, "水分");
            }

            ////
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    radarChart = chart.;
            //}


        }
    }
}

