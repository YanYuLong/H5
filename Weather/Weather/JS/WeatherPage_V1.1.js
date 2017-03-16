var latitude, longitude;   /*维度和经度信息*/
function screenHandler() {
    getLocation();
}
function getStyle(obj, attr) {
    return window.getComputedStyle ? window.getComputedStyle(obj, null)[attr] : obj.currentStyle[attr];
}

window.onload = screenHandler;

/*地理信息获取*/
function getLocation() {
    showPosition();
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);
    }
    else {
        alert("您当前的设备不支持地理定位");
    }
}
function showPosition(position) {
    latitude = position.coords.latitude;
    longitude = position.coords.longitude;
    ajaxLocation();
}
function showError(error) {
    switch (error.code) {
        case error.PERMISSION_DENIED:
            alert("您拒绝了使用地理定位。。我们只能使用IP地址进行预报可能具有不准确性");
            break;
        case error.POSITION_UNAVAILABLE:
            alert("无法获取您的位置！！");
            break;
        case error.TIMEOUT:
            alert("获取位置超时");
            break;
        case error.UNKNOWN_ERROR:
            alert("未知错误");
            break;
    }
}

var CityName;
//ajax 获取当前地理位置信息
function ajaxLocation() {
    var ajaxUrl = "/GetWeatherInfo.ashx?t=@@&location=";
    var xmlHttpRequest = new XMLHttpRequest();
    if (xmlHttpRequest) {
        var loc = latitude + "," + longitude;
        ajaxUrl = ajaxUrl.replace("@@", Math.random());
        var requestURL = ajaxUrl + loc;
        xmlHttpRequest.open("GET", requestURL, true);
        xmlHttpRequest.onreadystatechange = function () {
            if (xmlHttpRequest.readyState == 4 && xmlHttpRequest.status == 200) {
                //alert(xmlHttpRequest.responseText);
                var obj = eval("(" + xmlHttpRequest.responseText + ")");
                var info = "";
                CityName = obj.result.addressComponent.district ? obj.result.addressComponent.district : obj.result.addressComponent.city;
                var streetName=obj.result.addressComponent.street ?"("+ obj.result.addressComponent.street+")" : "";
                alert(CityName);
                CityName = CityName.replace("区", " ");
                CityName = CityName.replace("市", " ");
                CityName = CityName.replace("州", " ");
                CityName = CityName.replace("旗", " ");
                document.getElementById("locationName").innerText = CityName + streetName;
                ajaxWeatherInfo();
                
                //console.log(xmlHttpRequest.responseText);
            }
        }
        xmlHttpRequest.send();
    }
}

function ajaxWeatherInfo()
{
    var ajaxUrl = "/GetWeatherInfo.ashx?t=@@&cityName=";
    var xmlHttpRequest = new XMLHttpRequest();
    if (xmlHttpRequest) {
        ajaxUrl = ajaxUrl.replace("@@", Math.random());
        var requestURL = ajaxUrl + CityName;
        xmlHttpRequest.open("GET", requestURL, true);
        xmlHttpRequest.onreadystatechange = function () {
            if (xmlHttpRequest.readyState == 4 && xmlHttpRequest.status == 200) {
                var obj = eval("(" + xmlHttpRequest.responseText + ")");
                alert(xmlHttpRequest.responseText);
                setWeatherInfo(obj);
                //console.log(xmlHttpRequest.responseText);
            }
        }
        xmlHttpRequest.send();
    }
}

function setWeatherInfo(weatherInfo)
{
    var nowWeatherCode = document.getElementById("nowWeatherText").innerText = weatherInfo.info[0].now.cond.code;
    document.getElementById("nowWeatherPic").setAttribute("src", "http://files.heweather.com/cond_icon/"+nowWeatherCode+".png")
    document.getElementById("nowWeatherText").innerText = weatherInfo.info[0].now.cond.txt;
    document.getElementById("nowTmp").innerText = weatherInfo.info[0].now.tmp + "°"; 
    document.getElementById("fengLi").innerText = weatherInfo.info[0].now.wind.spd; 
    document.getElementById("fengXiang").innerText = weatherInfo.info[0].now.wind.dir; 
    document.getElementById("shiDu").innerHTML = weatherInfo.info[0].now.hum+"%<span>湿度</span>";
    document.getElementById("minTemp").innerHTML = weatherInfo.info[0].daily_forecast[0].tmp.min + "%<span>MIN</span>";
    document.getElementById("maxTemp").innerHTML = weatherInfo.info[0].daily_forecast[0].tmp.max + "%<span>MAX</span>";
}