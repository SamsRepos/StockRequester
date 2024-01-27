window.onload = function ()
{
    statusChange();
    document.getElementById("selectStatus").addEventListener("change", statusChange);
    document.getElementById("cancellationReason").addEventListener("keypress", validateCancellationReason);
};

function statusChange()
{
    console.log("Registering status change");

    var status = document.getElementById("selectStatus").value;

    var trackingInfoDiv = document.getElementById("trackingInfoDiv");
    trackingInfoDiv.style.display = (status === "Sent") ? "block" : "none";

    var cancellationReasonDiv = document.getElementById("cancellationReasonDiv");
    cancellationReasonDiv.style.display = (status === "Cancelled") ? "block" : "none";
    if (status === "Cancelled")
    {
        validateCancellationReason();
    }
}

function validateCancellationReason()
{
    console.log("Validating cancellation reason");

    var cancellationReasonInput = document.getElementById("cancellationReason");
    var status = document.getElementById("selectStatus").value;

    if (status === "Cancelled")
    {
        var value = cancellationReasonInput.value.trim();

        if (value === "" || isWhitespace(value))
        {
            cancellationReasonInput.classList.remove("is-valid");
            cancellationReasonInput.classList.add("is-invalid");
        }
        else
        {
            cancellationReasonInput.classList.remove("is-invalid");
            cancellationReasonInput.classList.add("is-valid");
        }
    }
}

function isWhitespace(str)
{
    console.log("Checking for whitespace");
    var len = str.trim().length;
    if (len === 0)
    {
        console.log("  - is whitespace");
        return true;
    }
    else
    {
        console.log("  - is not whitespace");
        return false;
    }
}

