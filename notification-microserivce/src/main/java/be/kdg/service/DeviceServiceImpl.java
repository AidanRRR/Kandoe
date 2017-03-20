package be.kdg.service;

import be.kdg.model.Device;
import be.kdg.persistence.DeviceRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

/**
 * Created by mathi on 9/02/2017.
 */
@Service
public class DeviceServiceImpl implements DeviceService {

    @Autowired
    DeviceRepository deviceRepository;

    @Override
    public Device store(Device device){
        return deviceRepository.save(device);
    }

    @Override
    public Device findByUserId(String userId){
        return deviceRepository.findByUserId(userId);
    }

}
