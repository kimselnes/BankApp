pageNo = 2;

document.addEventListener("DOMContentLoaded", function () {
    var button = document.getElementById("showMoreBtn");
    if (button) {
        button.addEventListener("click", showMoreTransactions);
    }
});

async function showMoreTransactions() {

    var button = document.getElementById("showMoreBtn");
    var body = document.getElementById("transactionBody");

    var response = await fetch("/Accounts/GetTransactions?id=" + body.dataset.accountId
        + "&pageNo=" + pageNo);
    var html = await response.text();
    var hasNext = response.headers.get("X-Has-Next") === "true";

    //var meta = document.querySelector("#transactionMeta");

    //if (meta !== null) {
    //    meta.remove();
    //}
    
    body.insertAdjacentHTML("beforeend", html);
    pageNo++;

    if (!hasNext && button !== null) {
        button.remove();
    }
};