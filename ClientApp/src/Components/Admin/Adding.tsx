import { Button, FormControl, Input, InputAdornment, InputLabel, TextField } from "@mui/material";
import AdminHeader from "./AdminHeader";
import "./Admin.css";

interface Props {
    window?: () => Window;
}

export default function Adding(props: Props){
    let menu;
    menu = (
        <div>

            <form className="form_adding">
                <div className="form_adding_div">
                    <TextField id="standard-basic" label="Name" variant="standard" />
                    <TextField id="standard-basic" label="Creator" variant="standard" />
                </div>
                <FormControl fullWidth sx={{ m: 1 }} variant="standard">
                    <TextField id="standard-basic" label="Date" variant="standard" />
                </FormControl>
                <div className="form_adding_div">
                    <TextField id="standard-basic" label="Info" variant="standard" />
                </div>
                <div className="form_adding_div">
                    <TextField id="standard-basic" label="Changelog" variant="standard" />
                </div>
                <div className="form_adding_div">
                    <TextField id="standard-basic" label="ok" minRows={5} variant="standard" />
                </div>
                      <Button className="btn-custom" type="submit">Add Transport</Button>
                      
          </form>
        </div>
    );

    return(
        <div>
          <AdminHeader prop={props} main={menu}></AdminHeader>
          
        </div>
    );
}