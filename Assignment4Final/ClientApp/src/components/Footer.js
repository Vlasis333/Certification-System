import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Button, NavDropdown } from 'react-bootstrap';

import { Link } from 'react-router-dom';
import './NavMenu.css';
// import { logout } from './auth/handleJWT';

//import { TiHome } from "react-icons/ti";

function Footer() {
    // const navigate = useNavigate();

    return (
        
          
                <Container>
                <hr/>
            <Navbar  bg="dark" variant="dark"    >
                <NavbarBrand >Footer</NavbarBrand>
                <NavbarBrand  >Footer</NavbarBrand>
                <NavbarBrand  >Footer</NavbarBrand>
                <NavbarBrand >Footer</NavbarBrand>


            </Navbar>
                </Container>
       
    );
}
export default Footer;
