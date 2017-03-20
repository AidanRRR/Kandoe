package be.kdg.controller;

import be.kdg.model.Device;
import be.kdg.service.DeviceService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

/**
 * Created by mathi on 9/02/2017.
 */
@RestController
@RequestMapping("/user")
public class DeviceController {

    @Autowired
    private DeviceService deviceService;

    @RequestMapping(value = "/add", method = RequestMethod.POST)
    public Device store(@RequestBody Device device){
        return deviceService.store(device);
    }
}
