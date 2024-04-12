var element = document.querySelector('.vector-header-container');
var element2 = document.querySelector('.mw-footer-container');
if (element) {
    element.remove();
    element2.remove();
    document.getElementById('left-navigation').remove();
    document.getElementById('right-navigation').remove();
    document.getElementById('p-lang-btn').remove();
}

var referencesWrap = document.querySelector('.mw-references-wrap .references');
if (referencesWrap) {
    var listItems = referencesWrap.querySelectorAll('li');
    listItems.forEach(function(item) {
        item.remove();
    });
}

var links = document.querySelectorAll('a');
console.log('Number of links: ' + links.length);
links.forEach(function(link) {
    var href = link.getAttribute('href');
    console.log('Link href: ' + href);
    // Check if the href attribute does not start with '/wiki', 'https://en.wikipedia.org', or '#cite_note'
    if (href && !href.startsWith('/wiki') && !href.startsWith('#cite') && !href.startsWith('#')) {
        link.remove();
    }
});