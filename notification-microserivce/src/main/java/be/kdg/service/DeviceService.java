package be.kdg.service;

import be.kdg.model.Device;

/**
 * Created by mathi on 9/02/2017.
 */
public interface DeviceService {
    Device store(Device device);

    Device findByUserId(String userId);


}
