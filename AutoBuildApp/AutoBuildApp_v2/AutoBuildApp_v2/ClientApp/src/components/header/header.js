import React from "react";
import Navbar from 'react-bootstrap/Navbar';
import Nav from 'react-bootstrap/Nav';
import { DropdownButton, NavDropdown } from "react-bootstrap";
import Button from "react-bootstrap/Button";
import './header.css';
import Dropdown from "react-bootstrap/Dropdown";

function Header(){
    return (
        <Navbar collapseOnSelect bg="dark" variant="dark">
            <Navbar.Brand href="/">Automated PC Builder</Navbar.Brand>
            <Navbar.Collapse id="responsive-navbar">
                <Nav className="apb-navbar">
                    <NavDropdown title="Builder" id="builder-dropdown">
                        <NavDropdown.Item href="/builder">Builder</NavDropdown.Item>
                        <NavDropdown.Divider />
                        <NavDropdown.Item href="/upgrader">Upgrader</NavDropdown.Item>
                    </NavDropdown>
                    <Nav.Link href="/most-popular-builds">Most Popular Builds</Nav.Link>
                    <Nav.Link href="/garage">Garage</Nav.Link>
                    <Nav.Link href="/catalog">Catalog</Nav.Link>
                    <Nav.Link href="/admin-portal" /*hidden*/>Administration Portal</Nav.Link>
                    <Nav.Link href="/inventory-management" /*hidden*/>Inventory Management</Nav.Link>
                </Nav>
                <div className="apb-login">
                    <Button className="custom-btn">Login</Button>
                    <Button className="custom-btn">Register</Button>
                    <DropdownButton /*hidden*/id="profile-button" title="User Name">
                        <Dropdown.Item className="dropdown-item">Profile</Dropdown.Item>
                        <Dropdown.Item className="dropdown-item">Settings</Dropdown.Item>
                        <Dropdown.Divider/>
                        <Dropdown.Item className="dropdown-item">Logout</Dropdown.Item>
                    </DropdownButton>
                </div>
            </Navbar.Collapse>
      </Navbar>
    );
}

export default Header;