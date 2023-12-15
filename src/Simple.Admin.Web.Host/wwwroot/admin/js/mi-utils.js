/**
 * 公共工具
 */
const MiUtils = {
    urlParmas: (name) => getQueryVariable(name),
    ajaxSucceed: (code) => validateSuccessfully(code),
    setGlobalParams: (obj) => setParams(obj),
    getGlobalParams: (obj) => getParams(obj),
    removeGlobalParams: (obj) => removeParams(obj),
    header: () => genMiHeader(),
    setItem: function (key, val) {
        localStorage.setItem(key, val)
    },
    removeItem: function (key) {
        localStorage.removeItem(key)
    },
    getItem: function (key) {
        return localStorage.getItem(key)
    }
}

/**
 * 响应码
 */
const MiResponseCode = {
    /** 成功 */
    Success: 10000,
    /** 错误请求，参数验证失败 */
    ParameterError: 10001,
    /** 未登录 */
    NonAuth: 10002,
    /** 禁止访问 */
    Forbidden: 10003,
    /** 找不到，不存在 */
    NonExist: 10004,
    /** 程序错误 */
    Error: 10005,
    /** 失败 */
    Fail: 10006,
    /** 请求频繁 */
    Fail: 10007
}

/**
 * 获取URL参数
 * @param {String} variable 参数名
 * @returns
 */
function getQueryVariable(variable) {
    let query = window.location.search.substring(1);
    let vars = query.split("&");
    for (let i = 0; i < vars.length; i++) {
        let pair = vars[i].split("=");
        if (pair[0] == variable) { return pair[1]; }
    }
    return '';
}

/**
 * 验证响应码是否成功
 * @param {any} code
 * @returns
 */
function validateSuccessfully(code) {
    return code === MiResponseCode.Success;
}

function setParams(obj) {
    let isObject = obj !== null && typeof obj === 'object'
    if (!isObject) {
        console.error('params "obj" is not Object')
        return;
    }
    let param = localStorage.getItem('mi-params')
    if (param) {
        let json = JSON.parse(param)
        json = { ...json, ...obj }
        localStorage.setItem('mi-params', JSON.stringify(json))
        return;
    }
    localStorage.setItem('mi-params', JSON.stringify(obj))
}

function getParams(key) {
    let param = localStorage.getItem('mi-params')
    if (param) {
        let json = JSON.parse(param)
        return json[key]
    }
    return null
}

function removeParams(key) {
    let param = localStorage.getItem('mi-params')
    if (param) {
        let json = JSON.parse(param)
        delete json[key]
        localStorage.setItem('mi-params', JSON.stringify(json))
    }
}

function genMiHeader() {
    let xhr = new XMLHttpRequest()
    xhr.open('get', 'http://ip-api.com/json/?lang=zh-CN', false)
    xhr.send()
    if (xhr.status == 200) {
        let data = JSON.parse(xhr.responseText)
        return `${data.country}-${data.regionName}-${data.city}&${data.query}`
    } else {
        console.error(xhr.responseText)
    }
}

/**
 * base64加密
 * @param {any} str
 * @returns
 */
function base64Encryption(str) {
    return window.btoa(encodeURI(str))
}

/**
 * base64解密
 * @param {any} str
 * @returns
 */
function base64Decrypt(str) {
    return decodeURI(window.atob(str))
}

function uuid() {
    var temp_url = URL.createObjectURL(new Blob());
    var uuid = temp_url.toString();
    URL.revokeObjectURL(temp_url);
    return uuid.substr(uuid.lastIndexOf("/") + 1);
}