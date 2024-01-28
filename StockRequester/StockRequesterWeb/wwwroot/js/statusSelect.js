window.onload = function ()
{
    statusChange();
    document.getElementById("selectStatus").addEventListener("change", statusChange);
    document.getElementById("cancellationReason").addEventListener("input", validateCancellationReason);
    document.getElementById("submitButton").addEventListener("click", handleSubmit);
};

function statusChange()
{
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
    var status = document.getElementById("selectStatus").value;

    if (status === "Cancelled")
    {
        var cancellationReasonInput = document.getElementById("cancellationReason");
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

function handleSubmit(event)
{
    var status = document.getElementById("selectStatus").value;

    if (status === "Cancelled") {
        var cancellationReasonInput = document.getElementById("cancellationReason");
        var value = cancellationReasonInput.value.trim();

        if (value === "" || isWhitespace(value)) {
            event.preventDefault(); // Preventing form submission
        }
    }
}