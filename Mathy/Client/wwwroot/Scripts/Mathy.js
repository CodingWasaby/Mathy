(function () {
    window.blazorLocalStorage = {
        get: key => key in localStorage ? JSON.parse(localStorage[key]) : null,
        set: (key, value) => { localStorage[key] = JSON.stringify(value); },
        delete: key => { delete localStorage[key]; }
    };
})();

function InitEditor(content) {
    var E = window.wangEditor;
    editor = new E('#div1');
    editor.customConfig.uploadImgMaxSize = 5 * 1024 * 1024;
    editor.customConfig.uploadImgShowBase64 = true;
    editor.customConfig.uploadImgServer = 'File/Upload';
    editor.create();    
    try {
        editor.txt.html(content);
    } catch (e) {
        editor.txt.text(content);
    }
}

function CloseLoginModal() {
    $("#CloseLoginModal").click();
}

function AlertMessage(msg) {
    $('#alertMessage').text(msg);
    $('#alertModel').modal('show');
}

function QuikStartClick() {
    $("#QuikStart").click();
}