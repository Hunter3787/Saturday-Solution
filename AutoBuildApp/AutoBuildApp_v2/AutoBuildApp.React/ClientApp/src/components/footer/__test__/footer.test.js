import React from 'react';
import Footer from '../footer';
import ReactDOM from 'react-dom';
import {act} from "react-dom/test-utils";

test("Renders without crashing", ()=>{
    const root = document.createElement("div");
    ReactDOM.render(<Footer />, root);

    expect(root.querySelector("h4").textContent).toBe("Pages");
    expect(root.querySelector("h4").textContent).toBe("About");

});
