window.onload = function () { statusChange(); };

document.getElementById("selectStatus").onchange = function() {statusChange()};

function statusChange() {
    var status = document.getElementById("selectStatus").value;
    
    var trackingDiv = document.getElementById("trackingInfo");
    trackingDiv.style.display = (status === "Sent") ? "block" : "none";
}
