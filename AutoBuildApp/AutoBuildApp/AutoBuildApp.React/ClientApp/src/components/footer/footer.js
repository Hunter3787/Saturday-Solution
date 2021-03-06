import React from 'react';
import './footer.css';

/*
Image logo on left
Site map on aligned right
Pages section will be a list of all normally accessible pages.
About will contain contact info and privacy policy.
*/
function Footer(){
    return (
        <footer class="footer">
            <nav class="footer-nav">
                <div class="footer-pages">
                    <h4>Pages</h4>
                    <ul class="footer-pages-ul">
                        <li>
                            <a href="/builder">Builder</a>
                        </li>
                        <li>
                            <a href="/upgrader">Upgrader</a>
                        </li>
                        <li>
                            <a href="/most-popular-builds">Most Popular Builds</a>
                        </li>
                        <li>
                            <a href="/garage">Garage</a>
                        </li>
                        <li>
                            <a href="/catalog">Catalog</a>
                        </li>
                    </ul>
                </div>
                <div class="footer-about">
                <h4>About</h4>
                    <ul class="footer-about-ul" style={{listStyleType: "none"}}>
                        <li>
                            <a href="/privacy-policy">Privacy Policy</a>
                        </li>
                        <li>
                            <a href="/about">About</a>
                        </li>
                        <li>
                            <a href="/contact">Contact Us</a>
                        </li>
                    </ul>
                </div>
            </nav>
        </footer>
    );
}

export default Footer;