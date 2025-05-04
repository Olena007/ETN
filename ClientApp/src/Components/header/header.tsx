import MoreIcon from '@mui/icons-material/MoreVert';
import * as React from 'react';
import {useState} from 'react';
import {styled, useTheme} from '@mui/material/styles';
import Box from '@mui/material/Box';
import Drawer from '@mui/material/Drawer';
import CssBaseline from '@mui/material/CssBaseline';
import MuiAppBar, {AppBarProps as MuiAppBarProps} from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';
import List from '@mui/material/List';
import Typography from '@mui/material/Typography';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import InboxIcon from '@mui/icons-material/MoveToInbox';
import MailIcon from '@mui/icons-material/Mail';
import {Avatar, Button, Menu, MenuItem, Tab, Tabs, Tooltip} from '@mui/material';
import {AccountCircle, Logout, PersonAdd, Settings} from '@mui/icons-material';
import {useTranslation} from 'react-i18next';
import './header.css';
import PublicIcon from '@mui/icons-material/Public';
import {Link, useNavigate} from 'react-router-dom';
import {red} from "@mui/material/colors";


const drawerWidth = 240;

const Main = styled('main', {shouldForwardProp: (prop) => prop !== 'open'})<{
    open?: boolean;
}>(({theme, open}) => ({
    flexGrow: 1,
    padding: theme.spacing(3),
    transition: theme.transitions.create('margin', {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
    }),
    marginLeft: `-${drawerWidth}px`,
    ...(open && {
        transition: theme.transitions.create('margin', {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
        marginLeft: 0,
    }),
}));

interface AppBarProps extends MuiAppBarProps {
    open?: boolean;
}

const AppBar = styled(MuiAppBar, {
    shouldForwardProp: (prop) => prop !== 'open',
})<AppBarProps>(({theme, open}) => ({
    transition: theme.transitions.create(['margin', 'width'], {
        easing: theme.transitions.easing.sharp,
        duration: theme.transitions.duration.leavingScreen,
    }),
    ...(open && {
        width: `calc(100% - ${drawerWidth}px)`,
        marginLeft: `${drawerWidth}px`,
        transition: theme.transitions.create(['margin', 'width'], {
            easing: theme.transitions.easing.easeOut,
            duration: theme.transitions.duration.enteringScreen,
        }),
    }),
}));

const DrawerHeader = styled('div')(({theme}) => ({
    display: 'flex',
    alignItems: 'center',
    padding: theme.spacing(0, 1),
    ...theme.mixins.toolbar,
    justifyContent: 'flex-end',
}));

type HeaderProps = {
    children: React.ReactNode;
};

export default function Header({children}: HeaderProps) {
    const theme = useTheme();
    const [open, setOpen] = React.useState(false);
    const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
    const [mobileMoreAnchorEl, setMobileMoreAnchorEl] = React.useState<null | HTMLElement>(null);
    const [anchorElLanguage, setAnchorElLanguage] = React.useState<null | HTMLElement>(null);
    const [mobileMoreAnchorElLanguage, setMobileMoreAnchorElLanguage] = React.useState<null | HTMLElement>(null);
    const {t, i18n} = useTranslation();
    const [logoutTheme, setLogout] = useState(false);
    const navigate = useNavigate();
    let auth;

    const onTranslate = (e: any) => {
        const language = e.target.value;
        i18n.changeLanguage(language);
    }

    const handleDrawerOpen = () => {
        setOpen(true);
    };
    const handleDrawerClose = () => {
        setOpen(false);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };
    const handleCloseLanguage = () => {
        setAnchorElLanguage(null);
    };
    const handleMenuClose = () => {
        setAnchorEl(null);
        handleMobileMenuClose();
    };
    const handleMenuCloseLanguage = () => {
        setAnchorElLanguage(null);
        handleMobileMenuCloseLanguage();
    };

    const isMenuOpen = Boolean(anchorEl);
    const isMobileMenuOpen = Boolean(mobileMoreAnchorEl);
    const isMenuOpenLanguage = Boolean(anchorElLanguage);

    const handleProfileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorEl(event.currentTarget);
    };
    const handleProfileMenuOpenLanguage = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElLanguage(event.currentTarget);
    };
    const handleMobileMenuClose = () => {
        setMobileMoreAnchorEl(null);
    };
    const handleMobileMenuCloseLanguage = () => {
        setMobileMoreAnchorElLanguage(null);
    };
    const handleMobileMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
        setMobileMoreAnchorEl(event.currentTarget);
    };
    const logout = () => {
        fetch('http://localhost:7229/api/logout', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            credentials: 'include',
        });
        window.location.reload();
        setLogout(true);
    }

    const [value, setValue] = React.useState('one');

    const handleChange = (event: React.SyntheticEvent, newValue: string) => {
        setValue(newValue);
    };
    

    if (logoutTheme == true) {
        auth = (
            <Box sx={{display: {xs: 'none', md: 'flex'}}}>
                <Tooltip title="Account settings">
                    <IconButton
                        onClick={handleProfileMenuOpen}
                        size="small"
                        sx={{ml: 2}}
                        aria-controls={open ? 'account-menu' : undefined}
                        aria-haspopup="true"
                        aria-expanded={open ? 'true' : undefined}
                    >
                        <Avatar sx={{width: 32, height: 32}}>M</Avatar>
                    </IconButton>
                </Tooltip>
            </Box>
        );
    } else {
        auth = (
            <Link to="/login" className="nav-link">Login</Link>
        );
    }

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
            transformOrigin={{horizontal: 'right', vertical: 'top'}}
            anchorOrigin={{horizontal: 'right', vertical: 'bottom'}}
        >
            <MenuItem onClick={handleClose}>
                <Avatar/> Profile
            </MenuItem>
            <MenuItem onClick={handleClose}>
                <Avatar/> My account
            </MenuItem>
            <Divider/>
            <MenuItem onClick={handleClose}>
                <ListItemIcon>
                    <PersonAdd fontSize="small"/>
                </ListItemIcon>
                Add another account
            </MenuItem>
            <MenuItem onClick={handleClose}>
                <ListItemIcon>
                    <Settings fontSize="small"/>
                </ListItemIcon>
                Settings
            </MenuItem>
            <MenuItem onClick={() => logout()}>
                <ListItemIcon>
                    <Logout fontSize="small"/>
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
            transformOrigin={{horizontal: 'right', vertical: 'top'}}
            anchorOrigin={{horizontal: 'right', vertical: 'bottom'}}
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

    const mobileMenuId = 'primary-search-account-menu-mobile';
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
                    <PublicIcon/>
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
                    <AccountCircle/>
                </IconButton>
                <p>{t("account")}</p>
            </MenuItem>
        </Menu>
    );


    return (
        <Box sx={{display: 'flex'}}>
            <CssBaseline/>
            <AppBar position="fixed" open={open} className="appbar">
                <Toolbar>
                    <IconButton
                        color="inherit"
                        aria-label="open drawer"
                        onClick={handleDrawerOpen}
                        edge="start"
                        sx={{mr: 2, ...(open && {display: 'none'})}}>
                        <MenuIcon/>
                    </IconButton>
                    <Box sx={{ flexGrow: 1, display: 'flex', justifyContent: 'center' }} onClick={() => navigate(`/`)}>
                        <Typography variant="h4" noWrap component="div">
                            <b>ETN</b>
                        </Typography>
                    </Box>

                    <Box sx={{display: {xs: 'none', md: 'flex'}}}>
                        <Tooltip title="Language settings">
                            <Button
                                onClick={handleProfileMenuOpenLanguage}
                                size="small"
                                sx={{ml: 2}}
                                aria-controls={open ? 'account-menu' : undefined}
                                aria-haspopup="true"
                                aria-expanded={open ? 'true' : undefined}
                                style={{color: "white"}}

                            >
                                {t("languages")}
                            </Button>
                        </Tooltip>
                    </Box>

                    {auth}
                    <Box sx={{display: {xs: 'flex', md: 'none'}}}>
                        <IconButton
                            size="large"
                            aria-label="show more"
                            aria-controls={mobileMenuId}
                            aria-haspopup="true"
                            onClick={handleMobileMenuOpen}
                            color="inherit"
                        >
                            <MoreIcon/>
                        </IconButton>
                    </Box>
                </Toolbar>

                <Box sx={{ height: '5px', backgroundColor: '#9e1b32', width: '100%' }}/>
                <Box sx={{ flexGrow: 1, display: 'flex', justifyContent: 'center' }} style={{backgroundColor: 'white'}}>
                    <Tabs
                        value={value}
                        onChange={handleChange}
                        aria-label="custom red tabs"
                        textColor="inherit"          
                        indicatorColor="secondary"   
                        sx={{
                            '& .MuiTabs-indicator': {
                                backgroundColor: '#9e1b32',
                            },
                            '& .MuiTab-root': {
                                color: '#9e1b32',
                                fontWeight: 500,
                            },
                            '& .MuiTab-root.Mui-selected': {
                                color: '#9e1b32',
                                borderBottom: '2px solid #9e1b32',
                            },
                            '& .MuiTab-root:hover': {
                                color: '#b71c1c',
                            },
                        }}
                    >
                    <Tab value="one" label="Home" />
                        <Tab value="two" label="Latest" />
                        <Tab value="three" label="For you" />
                    </Tabs>
                </Box>
                <Box sx={{ height: '2px', backgroundColor: '#979696', width: '100%' }}/>
            </AppBar>

            {renderMobileMenu}
            {renderMenu}
            {renderMenuLanguage}
            <Drawer
                sx={{
                    width: drawerWidth,
                    flexShrink: 0,
                    '& .MuiDrawer-paper': {
                        width: drawerWidth,
                        boxSizing: 'border-box',
                    },
                }}
                variant="persistent"
                anchor="left"
                open={open}>
                <DrawerHeader>
                    <IconButton onClick={handleDrawerClose}>
                        {theme.direction === 'ltr' ? <ChevronLeftIcon/> : <ChevronRightIcon/>}
                    </IconButton>
                </DrawerHeader>
                <Divider/>
                <List>
                    {['Inbox', 'Starred', 'Send email', 'Drafts'].map((text, index) => (
                        <ListItem key={text} disablePadding>
                            <ListItemButton>
                                <ListItemIcon>
                                    {index % 2 === 0 ? <InboxIcon/> : <MailIcon/>}
                                </ListItemIcon>
                                <ListItemText primary={text}/>
                            </ListItemButton>
                        </ListItem>
                    ))}
                </List>
                <Divider/>
                <List>
                    {['All mail', 'Trash', 'Spam'].map((text, index) => (
                        <ListItem key={text} disablePadding>
                            <ListItemButton>
                                <ListItemIcon>
                                    {index % 2 === 0 ? <InboxIcon/> : <MailIcon/>}
                                </ListItemIcon>
                                <ListItemText primary={text}/>
                            </ListItemButton>
                        </ListItem>
                    ))}
                </List>
            </Drawer>

            <Main open={open}>
                <DrawerHeader/>
                {children}
            </Main>
        </Box>
    );
}