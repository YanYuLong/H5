using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GetHttpInfo;

namespace Weather
{
    /// <summary>
    /// JsonP 的摘要说明
    /// </summary>
    public class Jsonp : IHttpHandler
    {
        //URL Template JsonP.ashx?type=(loactaion|weather)|(&locationdata=(data)|weatherdata=(data))&callback=(callbackmethod)
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string callBack = context.Request.QueryString["callback"];
            string type = context.Request.QueryString["type"];
            
            
            if (string.IsNullOrEmpty(callBack))
            {
                context.Response.Write("Error No CallBack Method");
                context.Response.Flush();
                return;
            }
            if (type != null)
            {
                if (type == "location")
                {
                    HandlerLocationReq(context,callBack);
                }
                else if (type == "weather")
                {
                    HandlerWeatherReq(context,callBack);
                }
                else
                {
                    context.Response.Write("Type Error");
                    context.Response.Flush();
                    return;
                }
            }
        }
        // 位置定位服务
        private void HandlerLocationReq(HttpContext context,string callback)
        {
            string locationdata = context.Request.QueryString["locationdata"];
            if (!string.IsNullOrEmpty(locationdata))
            {
                string locationJson = GetLocation(locationdata);
                string result = string.Format("{0}({1});", callback, locationJson);
                context.Response.Write(result);
            }
            else
            {
                context.Response.Write("Error Data Error");
            }
            context.Response.Flush();
            return;

        }
        //天气服务
        private void HandlerWeatherReq(HttpContext context,string callback)
        {
            string weatherdata = context.Request.QueryString["weatherdata"];
            if (!string.IsNullOrEmpty(weatherdata))
            {
                string weatherJson = getWeather(weatherdata);
                string result = string.Format("{0}({1});", callback, weatherJson);
                context.Response.Write(result);
            }
            else
            {
                context.Response.Write("Error Data Error");
            }
            context.Response.Flush();
            return;
        }

        private string GetLocation(string location)
        {
            GetHTTPInfo gti = new GetHTTPInfo();
            string locationJosn = gti.GetHttpResponseStr("api.map.baidu.com", "/geocoder/v2/?ak=VH2DX08lCi15Y1AxETUICD9zV8DnmHEg&location=" + location + "&output=json&pois=1");
            return locationJosn;
        }

        private string getWeather(string cityName)
        {
            int i = 0;
            string Json = "";
            Exception e = new Exception();

            GetHTTPInfo httpInfo = new GetHTTPInfo();
            try
            {
                string formateLocationInfo = System.Web.HttpUtility.UrlEncode(cityName);
                Json = httpInfo.GetHttpResponse("https://free-api.heweather.com/v5/weather?key=54084bfdb366446d85186ee1e80feb10&city=" + formateLocationInfo);
            }
            catch (Exception exp)
            {
                e = exp;
                return e.Message;
            }
            //AnalysisWeatherXML awx = new AnalysisWeatherXML();
            //WeatherInfo weatherInfo = new WeatherInfo();
            //try
            //{
            //    weatherInfo = awx.getWeatherInfo(xml);
            //}
            //catch (Exception exp)
            //{

            //}
            return Json;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}