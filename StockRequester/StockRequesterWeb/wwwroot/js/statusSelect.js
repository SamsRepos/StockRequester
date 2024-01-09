window.onload = function () {
    statusChange();
    document.getElementById("cancellation-reason").addEventListener("blur", validateCancellationReason);
    document.getElementById("selectStatus").addEventListener("change", statusChange);
};

document.querySelector("form").addEventListener("submit", function (event) {
    var status = document.getElementById("selectStatus").value;
    if (status === "Cancelled") {
        var cancellationReasonInput = document.getElementById("cancellation-reason");
        var value = cancellationReasonInput.value.trim();

        if (value === "" || isWhitespace(value)) {
            cancellationReasonInput.classList.remove("is-valid");
            cancellationReasonInput.classList.add("is-invalid");

            var errorMessage = document.getElementById("invalidCancellationReason");
            errorMessage.textContent = "Cancellation Reason is required.";

            event.preventDefault(); // Stop form submission
        } else {
            cancellationReasonInput.classList.remove("is-invalid");
            cancellationReasonInput.classList.add("is-valid");
        }
    }
});

function statusChange() {
    var status = document.getElementById("selectStatus").value;

    var trackingDiv = document.getElementById("trackingInfo");
    trackingDiv.style.display = (status === "Sent") ? "block" : "none";

    var cancellationDiv = document.getElementById("cancellationReason");
    cancellationDiv.style.display = (status === "Cancelled") ? "block" : "none";

    setCancellationReasonValidation(status);
    validateCancellationReason();
}

function validateCancellationReason() {
    var cancellationReasonInput = document.getElementById("cancellation-reason");
    var status = document.getElementById("selectStatus").value;

    if (status === "Cancelled") {
        var value = cancellationReasonInput.value.trim();

        if (value === "" || isWhitespace(value)) {
            cancellationReasonInput.classList.remove("is-valid");
            cancellationReasonInput.classList.add("is-invalid");
        } else {
            cancellationReasonInput.classList.remove("is-invalid");
            if (value.indexOf(" ") >= 0) {
                cancellationReasonInput.classList.add("is-invalid");
            } else {
                cancellationReasonInput.classList.add("is-valid");
            }
        }
    }
}


function setCancellationReasonValidation(status) {
    var cancellationReasonInput = document.getElementById("cancellation-reason");

    // Remove previous validation attributes
    cancellationReasonInput.removeAttribute("data-val");
    cancellationReasonInput.removeAttribute("data-val-required");

    if (status === "Cancelled") {
        // Add validation attributes for required field
        cancellationReasonInput.setAttribute("data-val", "true");
        cancellationReasonInput.setAttribute("data-val-required", "Cancellation Reason is required.");
    }
}

function isWhitespace(str) {
    console.log("Checking for whitespace")
    return str.trim().length === 0;
}

//b4 ai:
//window.onload = function () { statusChange(); };

//document.getElementById("selectStatus").onchange = function() {statusChange()};

//function statusChange() {
//    var status = document.getElementById("selectStatus").value;
    
//    var trackingDiv = document.getElementById("trackingInfo");
//    trackingDiv.style.display = (status === "Sent") ? "block" : "none";

//    var cancellationDiv = document.getElementById("cancellationReason");
//    cancellationDiv.style.display = (status === "Cancelled") ? "block" : "none";
//}
