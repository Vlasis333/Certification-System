import React, { useState, useContext,useEffect } from 'react';
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Button, NavDropdown } from 'react-bootstrap';
import { AuthenticationContext } from "./auth/AuthenticationContext";
import Authorized from "./auth/Authorized";
import { getToken, getUserId } from "./auth/handleJWT";

import axios from "axios";

import { AiOutlineCheck, AiOutlineClose, AiFillHome } from "react-icons/ai";


import { Link, useNavigate } from 'react-router-dom';
import './NavMenu.css';
import { logout } from './auth/handleJWT';

//import { TiHome } from "react-icons/ti";

function NavMenu() {
    const navigate = useNavigate();
    const { update, claims } = useContext(AuthenticationContext);
    const [claim, setClaim] = useState(claims.filter(x => x.name === "role")[0]?.value);
    const [isNew, SetIsNew] = useState(true);


    const getUserEmail = () => {
        const regex = /^[^@]+/;
        // const result = email.match(regex)[0];
        return claims.filter((x) => x.name === "email")[0]?.value.match(regex)[0];;
    };

    const [collapsed, setCollapsed] = useState(true);

    const toggleNavbar = () => {
        setCollapsed(!collapsed);
    };

    useEffect(() => {
        const token = getToken();
        if (token !== null) {

            axios.get(`https://localhost:7196/api/Candidate/${getUserId(token)}`).then((response) => {
                console.log(response.data)
                SetIsNew(false)
            }).catch(function (error) {
                console.log(error);
                SetIsNew(true)
            });
        }
    }, []);

    return (
        <header>
            <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" container light>
                <NavbarBrand tag={Link} to="/">
                    <h3><AiFillHome /></h3>
                </NavbarBrand>
                <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!collapsed} navbar>
                    <ul className="navbar-nav flex-grow">

                        <div className="d-flex">
                            <Authorized
                                role="admin"
                                authorized={<>
                                    <NavDropdown title="Marking">
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/marker">Mark an Exam</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/assigntomarker">Assign</NavLink>
                                        </NavDropdown.Item>
                                    </NavDropdown>

                                    <NavDropdown title="Item Banking">
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/certificate">Certificates List</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/certificate/create">Add Certificate</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/questions">Questions List</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/questions/create">Add Question</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/ExamsList">Examinations List</NavLink>
                                        </NavDropdown.Item>

                                    </NavDropdown>

                                    <NavDropdown title="User Management">
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/users">Users List</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/candidate">Candidates List</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/users/create">Add User</NavLink>
                                        </NavDropdown.Item>
                                    </NavDropdown>
                                </>}
                            />

                            {/* {isNew ?
                                <></>
                                :
                                <Authorized
                                    role="candidate"
                                    authorized={<>
                                        <NavItem>
                                            <NavLink tag={Link} className="text-dark" to="/certificate">Buy Certificate</NavLink>
                                        </NavItem>
                                        <NavItem>
                                            <NavLink tag={Link} className="text-dark" to="/candidate/availableexams">Available Exams</NavLink>
                                        </NavItem>
                                    </>}
                                />

                            } */}

                            <Authorized
                                role="qualitycontrol"
                                authorized={<>
                                    <NavDropdown title="View a list of ..">
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/marker">Exams</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/certificate">Certificates</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/questions">Questions</NavLink>
                                        </NavDropdown.Item>
                                        <NavDropdown.Item>
                                            <NavLink tag={Link} className="text-dark" to="/candidate">Candidates</NavLink>
                                        </NavDropdown.Item>
                                    </NavDropdown>
                                </>}
                            />
                            <Authorized
                                role="marker"
                                authorized={<>
                                    <NavItem>
                                        <NavLink tag={Link} className="text-dark" to="/marker">Actions</NavLink>
                                    </NavItem>
                                </>}
                            />

                            <Authorized
                                authorized={<>
                                    <span className="nav-link">{getUserEmail()}</span>
                                    <Button
                                        onClick={() => {
                                            navigate('/')
                                            logout();
                                            update([]);
                                        }}
                                        className="nav-link btn btn-link"
                                    >Log out</Button>
                                </>}
                                notAuthorized={<>
                                    <Link to="/register"
                                        className="nav-link btn btn-link">Register</Link>
                                    <Link to="/login"
                                        className="nav-link btn btn-link">Login</Link>
                                </>}
                            />
                        </div>
                    </ul>
                </Collapse>
            </Navbar>
        </header>
    );
}
export default NavMenu;
