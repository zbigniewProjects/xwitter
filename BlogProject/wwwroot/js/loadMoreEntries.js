function LoadEntries(url, stateObject, successFunc) {
    if (stateObject.cycle == null)
        stateObject.cycle = 0;

    // Return a new Promise
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: url + stateObject.cycle,
            type: 'GET',
            cache: false,
            contentType: 'application/json',
            success: function (response, textStatus, xhr) {
                stateObject.cycle++;
                // Call the success function with the response data
                successFunc(response.html, response.elementsCount,  response.elementsLeft);
                // Resolve the Promise
                resolve();
            },
            error: function (xhr, status, error) {
                console.error(xhr);  // Log the error
                // Reject the Promise with the xhr object or error
                reject(xhr);
            }
        });
    });
}

