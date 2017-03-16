using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using GetHttpInfo;

namespace Weather
{
    /// <summary>
    /// GetWeatherInfo 的摘要说明
    /// </summary>
    public class GetWeatherInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string location = context.Request.QueryString["location"];
            string cityName = context.Request.QueryString["cityName"];
            if (location != null)
            {
                string locationJson = GetLocation(location);
                context.Response.Write(locationJson);
                context.Response.End();
            }
            if (cityName != null)
            {
                string weatherInfo = getWeather(cityName);
                context.Response.Write(weatherInfo);
                context.Response.End();
            }
        }
        /// <summary>
        /// 获取指定城市的天气信息
        /// </summary>
        /// <param name="cityName">要获取天气信息的城市名称（绝对名称 不含有市、州之类的字符例如：北京 而不是北京市）</param>
        /// <returns></returns>
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
        /// <summary>
        /// 获取经纬度对应的地理 位置信息
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private string GetLocation(string location)
        {
            GetHTTPInfo gti = new GetHTTPInfo();
            string locationJosn = gti.GetHttpResponseStr("api.map.baidu.com", "/geocoder/v2/?ak=VH2DX08lCi15Y1AxETUICD9zV8DnmHEg&location=" + location + "&output=json&pois=1");
            return locationJosn;
        }

        //private string WeatherToJson(WeatherInfo weatherInfo)
        //{
        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //}
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}