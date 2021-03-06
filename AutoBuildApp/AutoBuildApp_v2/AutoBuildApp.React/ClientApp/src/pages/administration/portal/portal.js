import React from 'react';
import Spinner from 'react-bootstrap/Spinner';
import "./portal.css";

/*
Elements on the page:
    Image buttons with name:
        User Analysis Dashboard
        User Management(?)
*/
function Portal(){
    return (
    <div className="portal-wrapper">
        <h1>Shopping List for</h1>
        <ul>
            <li>Instagram</li>
            <li>WhatsApp</li>
            <li>Oculus</li>
            <Spinner animation="border" role="status">
            <span className="sr-only">Loading...</span>
        </Spinner>
        </ul>
        /*
        this spinner will show in place of the graph when it is loading 
        */
        


    </div>
    );
}

export default Portal;