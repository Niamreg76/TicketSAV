// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const textElement = document.getElementById("ecriture");
const speedElement = 2;
const ecriture = document.title;
const nouvelleEcriture = ecriture.slice(0, -15);
const titleLength = nouvelleEcriture.length;
let index = 1;
let speed = 300 / speedElement;
let longueur = 0;

const writeText = () => {
    textElement.innerHTML = ecriture.slice(0, index);
    index++;
    if (index > ecriture.length) index = 1;
    myTimeout = setTimeout(writeText, speed);
    longueur++;
    if (longueur >= titleLength)
        clearTimeout(myTimeout);
};

writeText();