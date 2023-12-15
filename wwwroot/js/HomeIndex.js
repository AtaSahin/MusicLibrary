//Limiti arttırma fonksiyonu
function loadMore() {
    var currentUrl = window.location.href;
    var searchTerm = new URLSearchParams(window.location.search).get('searchTerm');


    var currentLimit = parseInt(sessionStorage.getItem('limit')) || 8;


    var newLimit = currentLimit + 1;

    // Her basışta yeni bir URL oluştur...
    var newUrl = currentUrl.replace('limit=' + currentLimit, 'limit=' + newLimit);

    if (!newUrl.includes('limit=')) {
        newUrl += newUrl.includes('?') ? '&' : '?';
        newUrl += 'limit=' + newLimit;
    }


    if (searchTerm) {
        newUrl += '&searchTerm=' + searchTerm;
    }

    // Session olarak verileri kaydet
    sessionStorage.setItem('limit', newLimit);

    // Sayfa yenileme
    window.location.href = newUrl;
}