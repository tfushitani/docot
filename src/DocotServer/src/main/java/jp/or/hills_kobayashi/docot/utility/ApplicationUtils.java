package jp.or.hills_kobayashi.docot.utility;

import jp.or.hills_kobayashi.docot.model.Device;
import org.springframework.beans.BeanUtils;
import org.springframework.core.io.ClassPathResource;
import org.springframework.core.io.Resource;
import org.springframework.core.io.support.PropertiesLoaderUtils;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Properties;

public class ApplicationUtils {

    private static Properties properties = null;

    static {
        try {
            Resource resource = new ClassPathResource("/application.properties");
            properties = PropertiesLoaderUtils.loadProperties(resource);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public static Device hideSecret(Device device) {
        if (device == null) {
            return null;
        }
        Device returnDevice = new Device();
        BeanUtils.copyProperties(device, returnDevice);
        //returnDevice.setSecret("**********");
        return returnDevice;
    }

    public static List<Device> hideSecret(List<Device> devices) {
        List<Device> returnDevices = new ArrayList<Device>();
        for (Device device : devices) {
            returnDevices.add(hideSecret(device));
        }
        return returnDevices;
    }

    public static String getProperties(String key) {
        if (properties == null) {
            throw new RuntimeException();
        }
        return properties.getProperty(key);
    }

}
