async function downloadStreamReference(streamRef, blobType, fileName) {
    const stream = await streamRef.stream();
    var res = new Response(stream, { headers: { 'content-type': blobType } });
    var blob = await res.blob();
    var url = URL.createObjectURL(blob);

    createDownloadLink(url, fileName);

    URL.revokeObjectURL(url);
}

async function download(url, model) {
    var res = await fetch(url, {
        headers: {
            model
        }
    });
    var blob = await res.blob();
    var url = URL.createObjectURL(blob);

    createDownloadLink(url, 'sample.dat');

    URL.revokeObjectURL(url);
}

function createDownloadLink(url, fileName) {
    var link = document.createElement('a');
    link.download = fileName;
    link.setAttribute('href', url);
    link.target = "_self";
    link.click();
}