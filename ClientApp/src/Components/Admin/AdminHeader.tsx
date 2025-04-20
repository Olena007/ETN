import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import CssBaseline from '@mui/material/CssBaseline';
import Divider from '@mui/material/Divider';
import Drawer from '@mui/material/Drawer';
import IconButton from '@mui/material/IconButton';
import InboxIcon from '@mui/icons-material/MoveToInbox';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import MailIcon from '@mui/icons-material/Mail';
import MenuIcon from '@mui/icons-material/Menu';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import { Avatar, Button, Menu, MenuItem, Tooltip } from '@mui/material';
import { useTranslation } from 'react-i18next';
import PublicIcon from '@mui/icons-material/Public';
import { AccountCircle, Logout, PersonAdd, Settings, SwitchLeftTwoTone } from '@mui/icons-material';
import MoreIcon from '@mui/icons-material/MoreVert';
import { useState } from 'react';
import { Link } from 'react-router-dom';
import './Admin.css';
import DashboardIcon from '@mui/icons-material/Dashboard';
import AddCircleIcon from '@mui/icons-material/AddCircle';
import ListAltIcon from '@mui/icons-material/ListAlt';
import { Modal } from 'react-bootstrap';

const drawerWidth = 240;

interface Props {
  window?: () => Window;
}

function VersionsOnClick(){
  window.location.href = "admin/versions";
}

function DashboardOnClick(){
  window.location.href = "admin/dashboard";
}

export default function AdminHeader(props: {prop: Props, main: any}) {
    const [open, setOpen] = React.useState(false);
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const [anchorElLanguage, setAnchorElLanguage] = React.useState<null | HTMLElement>(null);
    const [mobileMoreAnchorElLanguage, setMobileMoreAnchorElLanguage] = React.useState<null | HTMLElement>(null);
    const [mobileMoreAnchorEl, setMobileMoreAnchorEl] = React.useState<null | HTMLElement>(null);
    const { t, i18n } = useTranslation();
    const { window } = props.prop;
    const [mobileOpen, setMobileOpen] = React.useState(false);
    const isMenuOpen = Boolean(anchorEl);
    const isMenuOpenLanguage = Boolean(anchorElLanguage);
    const isMobileMenuOpen = Boolean(mobileMoreAnchorEl);
    const mobileMenuId = 'primary-search-account-menu-mobile';
    const container = window !== undefined ? () => window().document.body : undefined;

    //Add
    const [show, setShow] = useState(false);
    const handleCloseMenu = () => setShow(false);
    const handleShowMenu = () => setShow(true);
    const submitAdd = () => {
      async function CreateVersion(){
        try {
        //   const response = await fetch('http://localhost:7229/Transport', {
        //     method: 'POST',
        //     headers: {
        //       'Content-Type': 'application/json',
        //       Accept: 'application/json',
        //     },
        //     body: JSON.stringify({transportId: 0, type: transType, transportNumber: transNumber, cityName: city})
        //   });
      
        //   if (!response.ok) {
        //     throw new Error(`Error! status: ${response.status}`);
        //   }
      
        //   const result = (await response.json()) as ITransport;
      
        //   console.log('result is: ', JSON.stringify(result, null, 4));
        //   //alert("added");
        //  // return result;
        } catch (error) {
          if (error instanceof Error) {
            console.log('error message: ', error.message);
            return error.message;
          } else {
            console.log('unexpected error: ', error);
            return 'An unexpected error occurred';
          }
        }
        CreateVersion();
      }
    }

    const onTranslate = (e:any) => {
      const language = e.target.value;
      i18n.changeLanguage(language);
    }

    

    const handleDrawerToggle = () => {
        setMobileOpen(!mobileOpen);
    };
    const handleProfileMenuOpenLanguage = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElLanguage(event.currentTarget);
    };
    const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };
    const handleMobileMenuCloseLanguage = () => {
        setMobileMoreAnchorElLanguage(null);
    };
    const handleMenuClose = () => {
        setAnchorEl(null);
        handleMobileMenuClose();
    };
    const handleMenuCloseLanguage = () => {
        setAnchorElLanguage(null);
        handleMobileMenuCloseLanguage();
    };
    const handleMobileMenuClose = () => {
        setMobileMoreAnchorEl(null);
    };
    const handleMobileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setMobileMoreAnchorEl(event.currentTarget);
    };
    const renderMenu = (
        <Menu
        anchorEl={anchorEl}
        id="account-menu"
        open={isMenuOpen}
        onClose={handleMenuClose}
        onClick={handleMenuClose}
        PaperProps={{
        elevation: 0,
        sx: {
            overflow: 'visible',
            filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
            mt: 1.5,
            '& .MuiAvatar-root': {
            width: 32,
            height: 32,
            ml: -0.5,
            mr: 1,
            },
            '&:before': {
            content: '""',
            display: 'block',
            position: 'absolute',
            top: 0,
            right: 14,
            width: 10,
            height: 10,
            bgcolor: 'background.paper',
            transform: 'translateY(-50%) rotate(45deg)',
            zIndex: 0,
            },
        },
        }}
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
        >
        <MenuItem onClick={handleClose}>
        <Avatar /> Profile
        </MenuItem>
        <MenuItem onClick={handleClose}>
        <Avatar /> My account
        </MenuItem>
        <Divider />
        <MenuItem onClick={handleClose}>
        <ListItemIcon>
            <PersonAdd fontSize="small" />
        </ListItemIcon>
        Add another account
        </MenuItem>
        <MenuItem onClick={handleClose}>
        <ListItemIcon>
            <Settings fontSize="small" />
        </ListItemIcon>
        Settings
        </MenuItem>
        <MenuItem onClick={handleClose}>
        <ListItemIcon>
            <Logout fontSize="small" />
        </ListItemIcon>
        Logout
        </MenuItem>
        </Menu>
    );
    const renderMenuLanguage = (
        <Menu
        anchorEl={anchorElLanguage}
        id="language-menu"
        open={isMenuOpenLanguage}
        onClose={handleMenuCloseLanguage}
        onClick={handleMenuCloseLanguage}
        PaperProps={{
        elevation: 0,
        sx: {
            overflow: 'visible',
            filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
            mt: 1.5,
            '& .MuiAvatar-root': {
            width: 32,
            height: 32,
            ml: -0.5,
            mr: 1,
            },
            '&:before': {
            content: '""',
            display: 'block',
            position: 'absolute',
            top: 0,
            right: 14,
            width: 10,
            height: 10,
            bgcolor: 'background.paper',
            transform: 'translateY(-50%) rotate(45deg)',
            zIndex: 0,
            },
        },
        }}
        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}
        >
            <MenuItem>
            <Button onClick={onTranslate} value="en" style={{color: "#00344A"}}>
            {t("english")}
            </Button>
            </MenuItem>
            <MenuItem>
            <Button onClick={onTranslate} value="ua" style={{color: "#00344A"}}>
            {t("ukranian")}
            </Button>
            </MenuItem>
            <MenuItem>
            <Button onClick={onTranslate} value="ru" style={{color: "#00344A"}}>
            {t("russian")}
            </Button>
            </MenuItem>
            
            
            
        </Menu>
    );
    // Mobile menu
    const renderMobileMenu = (
    <Menu
      anchorEl={mobileMoreAnchorEl}
      anchorOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      id={mobileMenuId}
      keepMounted
      transformOrigin={{
        vertical: 'top',
        horizontal: 'right',
      }}
      open={isMobileMenuOpen}
      onClose={handleMobileMenuClose}
    >

      <MenuItem onClick={handleProfileMenuOpenLanguage}>
        <IconButton
          size="large"
          aria-label="account of current user"
          aria-controls="primary-search-account-menu"
          aria-haspopup="true"
          color="inherit"
        >
          <PublicIcon />
        </IconButton>
        <p>{t("languages")}</p>
      </MenuItem>

      <MenuItem onClick={handleProfileMenuOpen}>
        <IconButton
          size="large"
          aria-label="account of current user"
          aria-controls="primary-search-account-menu"
          aria-haspopup="true"
          color="inherit"
        >
          <AccountCircle />
        </IconButton>
        <p>{t("account")}</p>
      </MenuItem>
    </Menu>
    );
    const drawer = (
        <div>
        <Toolbar />
        <Divider />
        <List>
          <Link to="/admin/dashboard" className='LinkGen'>
          <ListItem disablePadding>
              <ListItemButton>
                <ListItemIcon>
                <DashboardIcon />
                </ListItemIcon>
                <ListItemText primary={t("dashboard")} />
              </ListItemButton>
          </ListItem>
          </Link>
          <Link to="/admin/versions" className='LinkGen'>
            <ListItem disablePadding>
              <ListItemButton>
                <ListItemIcon>
                <ListAltIcon />
                </ListItemIcon>
                <ListItemText primary={t("versions")} />
              </ListItemButton>
          </ListItem>
          </Link>
        
          
        </List>
        <Divider />
        <Link to="/admin/adding" className='LinkGen'>
          <ListItem disablePadding>
              <ListItemButton>
                <ListItemIcon>
                <AddCircleIcon />
                </ListItemIcon>
                <ListItemText primary={"Add Version"} />
              </ListItemButton>
          </ListItem>
        </Link>

        {/* <Modal className="down" show={show} onHide={handleCloseMenu}>
                              
          <Modal.Header closeButton>

            <Modal.Title>Add new transport</Modal.Title>
          </Modal.Header>
          <Modal.Body>
          <form onSubmit={submitAdd}>
                      <div className="form-add">
                          <h6 className="mb-3 fw-normal">Type</h6>
                          <input type="text" className="form-control" placeholder="Transport Type" required 
                          />
                      </div>
                      <div className="form-add">
                          <h6 className="mb-3 fw-normal">Number</h6>
                          <input type="text" className="form-control" placeholder="Transport Number" required 
                          />
                      </div>
                      <div className="form-add">
                          <h6 className="mb-3 fw-normal">City</h6>
                          <input type="text" className="form-control" placeholder="City Name" required 
                          />
                      </div>
                      <Button className="btn-custom" type="submit">Add Transport</Button>
                      
          </form>
          </Modal.Body>
        </Modal> */}
        </div>
    );




  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />    
      <AppBar
        position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}
        className="appbar"
      >
        <Toolbar>
          <IconButton
            color="inherit"
            aria-label="open drawer"
            edge="start"
            onClick={handleDrawerToggle}
            sx={{ mr: 2, display: { sm: 'none' } }}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" noWrap component="div">
            Carsharing
          </Typography>


         <Box sx={{ flexGrow: 1 }} />

    <Box sx={{ display: { xs: 'none', md: 'flex' } }}>
          <Tooltip title="Language settings">
           <Button
            onClick={handleProfileMenuOpenLanguage}
            size="small"
            sx={{ ml: 2 }}
            aria-controls={open ? 'account-menu' : undefined}
            aria-haspopup="true"
            aria-expanded={open ? 'true' : undefined}
            style={{color: "white"}}
        
          >
            {t("languages")}
          </Button>

        </Tooltip>
    </Box>

             <Box sx={{ display: { xs: 'none', md: 'flex' } }}>
           <Tooltip title="Account settings">
           <IconButton
            onClick={handleProfileMenuOpen}
            size="small"
            sx={{ ml: 2 }}
            aria-controls={open ? 'account-menu' : undefined}
            aria-haspopup="true"
            aria-expanded={open ? 'true' : undefined}
          >
            <Avatar sx={{ width: 32, height: 32 }}>M</Avatar>
          </IconButton>
        </Tooltip>
        </Box>

    {/* Small screen */}
    <Box sx={{ display: { xs: 'flex', md: 'none' } }}>
        <IconButton
        size="large"
        aria-label="show more"
        aria-controls={mobileMenuId}
        aria-haspopup="true"
        onClick={handleMobileMenuOpen}
        color="inherit"
    >
        <MoreIcon />
    </IconButton>
    </Box>
        </Toolbar>
      </AppBar>
      {renderMenuLanguage}
      {renderMenu}
      {renderMobileMenu}
      <Box
        component="nav"
        sx={{ width: { sm: drawerWidth }, flexShrink: { sm: 0 } }}
        aria-label="mailbox folders"
      >
        {/* The implementation can be swapped with js to avoid SEO duplication of links. */}
        <Drawer
          container={container}
          variant="temporary"
          open={mobileOpen}
          onClose={handleDrawerToggle}
          ModalProps={{
            keepMounted: true, // Better open performance on mobile.
          }}
          sx={{
            display: { xs: 'block', sm: 'none' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
          }}
        >
          {drawer}
        </Drawer>
        <Drawer
          variant="permanent"
          sx={{
            display: { xs: 'none', sm: 'block' },
            '& .MuiDrawer-paper': { boxSizing: 'border-box', width: drawerWidth },
          }}
          open
        >
          {drawer}
        </Drawer>
      </Box>
      <Box
        component="main"
        sx={{ flexGrow: 1, p: 3, width: { sm: `calc(100% - ${drawerWidth}px)` } }}
      >
        <Toolbar />
        {props.main}
      </Box>
    </Box>
  );
}
