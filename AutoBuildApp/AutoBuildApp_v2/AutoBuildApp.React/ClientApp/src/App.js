import React from "react";
import Table from "react-bootstrap/Table";
import { BrowserRouter as Router, Switch, Route} from "react-router-dom";
//Page Imports
import About from "./pages/about/about";
import Footer from "./components/footer/footer";
import Header from "./components/header/header";
import PrivacyPolicy from "./pages/privacy-policy/privacyPolicy";
import Builds from './pages/most-popular-builds/builds';
import Portal from "./pages/administration/portal/portal"
import Inventory from "./pages/administration/managers/inventoryManager"

//Fix below.
import Login from './pages/login/login';

import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";


const buildList = [
  {
    id: 1,
    title: "Excellent AMD Gaming/Streaming Build",
    cpu: "AMD Ryzen 5 5600X",
    cpuCooler: "ARCTIC Freezer 34 eSports DUO CPU Cooler",
    gpu: "PNY GeForce RTX 3070 8 GB UPRISING Video Card",
    motherboard: "MSI B550-A PRO ATX AM4 Motherboard",
    memory: "Crucial Ballistix 16 GB (2x 8 GB) DDR4-3600 CL16 Memory",
    storage: "Team MP33 1 TB M.2-2280 NVME Solid State Drive",
    case: "Corsair 4000D Aiirflo ATX Mid Tower Case",
    psu: "EVGA 650 W 80+ Gold Certified Semi-modular ATX Power Supply",
  },
  {
    id: 2,
    title: "Excellent AMD Gaming/Streaming Build",
    cpu: "AMD Ryzen 5 5600X",
    cpuCooler: "ARCTIC Freezer 34 eSports DUO CPU Cooler",
    gpu: "PNY GeForce RTX 3070 8 GB UPRISING Video Card",
    motherboard: "MSI B550-A PRO ATX AM4 Motherboard",
    memory: "Crucial Ballistix 16 GB (2x 8 GB) DDR4-3600 CL16 Memory",
    storage: "Team MP33 1 TB M.2-2280 NVME Solid State Drive",
    case: "Corsair 4000D Aiirflo ATX Mid Tower Case",
    psu: "EVGA 650 W 80+ Gold Certified Semi-modular ATX Power Supply",
  },
];

function App() {
  return (
    <div className="app">
      <Router>
        <Header />
        <div>
          <Switch>
            <Route path="/inventory-management">
              <Inventory />
            </Route>
            <Route path="/admin-portal">
              <Portal />
            </Route>
            <Route path="/privacy-policy">
              <PrivacyPolicy />
            </Route>
            <Route path="/contact">
              <PlaceHolder/>
            </Route>
            <Route path="/about">
              <About />
            </Route>
            <Route path="/builder">
              <Tables builds={buildList}/>
            </Route>
            <Route path="/catalog">
              <PlaceHolder />
            </Route>
            <Route path="/garage">
              <PlaceHolder />
            </Route>
            <Route path="/most-popular-builds">
              <PlaceHolder />
            </Route>
            <Route path="/">
              <Builds builds={buildList}/>
            </Route>
          </Switch>
        </div>
        <Footer />
      </Router>
    </div>
  );
}

function PlaceHolder(){
  return <div>Place Holder Page</div>
}



function Home() {
  return <div>Home Page</div>;
}

function Tables(props) {
  function _renderHeader() {
    const columnHeaderKeys = Object.keys(props.builds[0]);
    return (
      <thead>
        <tr>
          {columnHeaderKeys.map(function (key) {
            if (key == "id") {
              return;
            }
            return (
              <th key={key}>{key.charAt(0).toUpperCase() + key.slice(1)}</th>
            );
          })}
        </tr>
      </thead>
    );
  }
  
  function _renderBody() {
    function _renderBuild(build) {
      const buildKeys = Object.keys(build)
      return (
        <tr>
          {buildKeys.map(function (key) {
            if (key == "id") {
              return;
            }
            return <td key={key}>{build[key]}</td>;
          })}
        </tr>
      );
    }

    const builds = props.builds;
    return (
      <tbody>
        {builds.map(function (build) {
          return _renderBuild(build)
        })}
      </tbody>
    );
  }

  return (
    <Table striped bordered hover size="sm">
      {_renderHeader()}
      {_renderBody()}
    </Table>
  );
}

export default App;
