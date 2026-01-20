$(document).ready(function () {

    // Load Dashboard by default
    loadPage('/Dashboard/Home');

    // Sidebar click handler (event delegation ✔)
    $('.sidebar').on('click', '.nav-link', function () {

        $('.sidebar .nav-link').removeClass('active');
        $(this).addClass('active');

        const url = $(this).data('url');
        loadPage(url);
    });

});

// =============================
// Common AJAX Page Loader
// =============================
function loadPage(url) {
    $.ajax({
        url: url,
        type: 'GET',
        beforeSend: function () {
            $('#target').html(`
                <div class="text-center p-5">
                    <div class="spinner-border text-primary"></div>
                </div>
            `);
        },
        success: function (result) {
            $('#target').html(result);
        },
        error: function () {
            $('#target').html(
                '<div class="alert alert-danger">Failed to load data</div>'
            );
        }
    });
}

// =============================
// Save Expense
// =============================
function saveExpense() {

    const amount = parseFloat($("#amount").val());
    const categoryId = parseInt($("#categoryId").val());
    const description = $("#description").val();

    if (!amount || amount <= 0) {
        alert("Please enter a valid amount");
        return;
    }

    if (!categoryId) {
        alert("Please select a category");
        return;
    }

    const data = {
        amount: amount,
        categoryId: categoryId,
        description: description
    };

    $.ajax({
        url: "/Dashboard/AddExpense",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (html) {

            $("#addExpenseModal").modal("hide");
            $("#expenseForm")[0].reset();

            // Reload dashboard after insert
            loadPage('/Dashboard/Home');
        },
        error: function () {
            alert("Failed to add expense");
        }
    });
}

//Get Categories
function loadCategories() {

    $.ajax({
        url: "/Dashboard/GetCategories",
        type: "GET",
        success: function (data) {

            const ddl = $("#categoryId");
            ddl.empty();
            ddl.append('<option value="">-- Select Category --</option>');

            $.each(data, function (i, item) {
                ddl.append(
                    `<option value="${item.categoryId}">
                        ${item.categoryName}
                     </option>`
                );
            });
        },
        error: function () {
            alert("Unable to load categories");
        }
    });
}
