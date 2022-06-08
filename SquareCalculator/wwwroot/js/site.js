$(document).ready(function () {
    updateValuesSpan();
    updateHistoryDiv();
});

function addHistory(result, isError = false) {

    let values = JSON.parse(localStorage.getItem("values")) || new Array();
    let history = JSON.parse(localStorage.getItem("history")) || new Array();

    history.push({ request: JSON.stringify(values), response: result, error: isError });
    localStorage.setItem("history", JSON.stringify(history))

    localStorage.removeItem("values");
    updateValuesSpan();
    updateHistoryDiv();
}

function updateValuesSpan() {
    let valuesSpan = $('#values');
    valuesSpan.text(JSON.parse(localStorage.getItem("values")));
}

function updateHistoryDiv() {
    let historyDiv = $('#history');
    let history = JSON.parse(localStorage.getItem("history")) || new Array();
    let result = "";
    history.forEach(item => result +=
        `<div class="row mx-1 px-5 pb-3 w-80">
            <div class="col mx-auto">
                <h5 class="my-2">` + item.request + `</h5>
                <p class="my-2 ` + (item.error ? 'text-danger' : 'text-success') + `">` + item.response + `</p>
            </div>
        </div>`)
    historyDiv.html(result);
}

function addNumber() {
    let valuesInput = $('#addNumberInput');
    if (!valuesInput.val()) return;

    let values = JSON.parse(localStorage.getItem("values")) || new Array();
    values.push(parseInt(valuesInput.val()));
    localStorage.setItem("values", JSON.stringify(values))

    valuesInput.val("");

    updateValuesSpan();
}

$('#addNumberInput').on('keypress', function (e) {
    if (e.which == 13)
        addNumber();
});

$("#addNumberButton").on('click', function () {
    addNumber();
});

$("#clearValuesButton").on('click', function () {
    localStorage.removeItem("values");
    updateValuesSpan();
});


$("#submitButton").on('click', function () {
    event.preventDefault();
    $.ajax({
        url: '/calculator/square',
        method: 'post',
        contentType: 'application/json',
        data: JSON.stringify({ values: (JSON.parse(localStorage.getItem("values")) || new Array()) }),
        success: function (data) {
            if (data['status'] == "error")
                addHistory(data["errorMessage"], true);
            else
                addHistory(data["result"]);
        }
    });
});