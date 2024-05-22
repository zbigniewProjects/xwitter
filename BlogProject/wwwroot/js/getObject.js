function GetObject(url) {
    return new Promise((resolve, reject) => {
        $.ajax({
            url: url,
            type: 'GET',
            cache: false,
            contentType: 'application/json',
            success: function (response, textStatus, xhr) {
                resolve(response);  // Resolve the promise with the response object
            },
            error: function (xhr, status, error) {
                reject(xhr);  // Reject the promise with the xhr object or error
            }
        });
    });
}