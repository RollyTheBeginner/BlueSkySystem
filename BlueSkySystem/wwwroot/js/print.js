function printForm(selectedId) {
    var allAdvances = document.getElementsByClassName('cash-advance-container');
    var printContents = '';

    // Find the selected cash advance container
    for (var i = 0; i < allAdvances.length; i++) {
        if (allAdvances[i].dataset.id === selectedId) {
            printContents = allAdvances[i].innerHTML;
            break;
        }
    }

    // If a matching cash advance is found, create a new window for printing
    if (printContents) {
        var originalContents = document.body.innerHTML;
        var printWindow = window.open('', '_blank');

        printWindow.document.write(`
            <html>
                <head>
                    <title>Print Cash Advance</title>
                    <link rel="stylesheet" href="/css/print.css" media="print" />
                </head>
                <body>
                    ${printContents}
                </body>
            </html>
        `);

        printWindow.document.close();  // Close the document to finish writing

        // Close the print window after printing
        printWindow.addEventListener('afterprint', function () {
            printWindow.close();
        });

        printWindow.print();            // Trigger the print dialog
    } else {
        alert('No matching cash advance found.');
    }
}

function selectCashAdvance() {
    var selectedId = document.getElementById('cashAdvanceSelector').value;
    document.getElementById('printButton').disabled = selectedId === '';
}
